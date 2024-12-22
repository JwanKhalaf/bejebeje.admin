using Amazon.S3;
using Amazon.S3.Transfer;
using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace bejebeje.admin.Application.Artists.Commands.SetImage;

public class SetArtistImageCommand : IRequest<bool>
{
    public int ArtistId { get; set; }

    [FromForm(Name = "artistImage")] public IFormFile ArtistImage { get; set; }
}

public class SetArtistImageCommandHandler : IRequestHandler<SetArtistImageCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName = "bejebeje.com";

    public SetArtistImageCommandHandler(IApplicationDbContext context, IAmazonS3 s3Client)
    {
    }

    public async Task<bool> Handle(SetArtistImageCommand command, CancellationToken cancellationToken)
    {
        // make sure the artist exists
        var artist = await _context.Artists.FindAsync(new object[] { command.ArtistId }, cancellationToken);
        if (artist == null)
        {
            throw new NotFoundException(nameof(Artist), command.ArtistId);
        }
        
        // validate file type
        var validFileTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!validFileTypes.Contains(command.ArtistImage.ContentType))
        {
            throw new InvalidOperationException("Invalid file type. Only JPG, PNG, and WebP are allowed.");
        }

        // load image using ImageSharp
        await using var stream = command.ArtistImage.OpenReadStream();
        
        // detect the format
        var format = await Image.DetectFormatAsync(stream, cancellationToken);
        if (format == null)
        {
            throw new InvalidOperationException("Unsupported or invalid image format.");
        }

        // reset stream position after detecting format
        stream.Position = 0;
        using var image = await Image.LoadAsync(stream, cancellationToken);

        // validate dimensions and squareness
        if (image.Width != image.Height)
        {
            throw new InvalidOperationException("The image must be square.");
        }

        if (image.Width < 500 || image.Height < 500)
        {
            throw new InvalidOperationException("The image must have minimum dimensions of 500x500 pixels.");
        }

        // process and upload images
        await ProcessAndUploadImage(image, command.ArtistId, 300, 300, cancellationToken);
        await ProcessAndUploadImage(image, command.ArtistId, 80, 80, cancellationToken);
        await ProcessAndUploadImage(image, command.ArtistId, 60, 60, cancellationToken);
        
        artist.HasImage = true;
        artist.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task ProcessAndUploadImage(
        Image image, int artistId, int width, int height, CancellationToken cancellationToken)
    {
        // create resized image
        using var resizedImage = image.Clone(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(width, height), Mode = ResizeMode.Crop
        }));

        // save as jpg
        await UploadToS3(resizedImage, artistId, width, height, "jpg", new JpegEncoder { Quality = 60 },
            cancellationToken);

        // save as webp
        await UploadToS3(resizedImage, artistId, width, height, "webp", new WebpEncoder { Quality = 60 },
            cancellationToken);
    }

    private async Task UploadToS3(
        Image image, int artistId, int width, int height, string extension, IImageEncoder encoder,
        CancellationToken cancellationToken)
    {
        // determine file name suffix based on size
        string sizeSuffix = width switch
        {
            300 => "s",
            80 => "sm",
            60 => "xsm",
            _ => throw new InvalidOperationException("Unexpected image size.")
        };

        // generate s3 key (e.g., "artist-images/<artist-id>-s.jpg")
        var fileName = $"{artistId}-{sizeSuffix}.{extension}";
        var key = $"artist-images/{fileName}";

        // save image to a memory stream
        using var memoryStream = new MemoryStream();
        image.Save(memoryStream, encoder);
        memoryStream.Position = 0;

        // upload to s3
        var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(
            new TransferUtilityUploadRequest
            {
                InputStream = memoryStream,
                BucketName = _bucketName,
                Key = key,
                ContentType = $"image/{extension}"
            }, cancellationToken);
    }
}