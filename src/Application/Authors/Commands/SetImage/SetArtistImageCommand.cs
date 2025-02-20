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

namespace bejebeje.admin.Application.Authors.Commands.SetImage;

public class SetAuthorImageCommand : IRequest<bool>
{
    public int AuthorId { get; set; }

    [FromForm(Name = "authorImage")] public IFormFile AuthorImage { get; set; }
}

public class SetAuthorImageCommandHandler : IRequestHandler<SetAuthorImageCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName = "bejebeje.com";

    public SetAuthorImageCommandHandler(IApplicationDbContext context, IAmazonS3 s3Client)
    {
        _context = context;
        _s3Client = s3Client;
    }

    public async Task<bool> Handle(SetAuthorImageCommand command, CancellationToken cancellationToken)
    {
        // make sure the author exists
        var author = await _context.Authors.FindAsync(new object[] { command.AuthorId }, cancellationToken);
        if (author == null)
        {
            throw new NotFoundException(nameof(Author), command.AuthorId);
        }

        // validate file type
        var validFileTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!validFileTypes.Contains(command.AuthorImage.ContentType))
        {
            throw new InvalidOperationException("Invalid file type. Only JPG, PNG, and WebP are allowed.");
        }

        // load image using ImageSharp
        await using var stream = command.AuthorImage.OpenReadStream();

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
        await ProcessAndUploadImage(image, command.AuthorId, 300, 300, cancellationToken);
        await ProcessAndUploadImage(image, command.AuthorId, 80, 80, cancellationToken);
        await ProcessAndUploadImage(image, command.AuthorId, 60, 60, cancellationToken);

        author.HasImage = true;
        author.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task ProcessAndUploadImage(
        Image image, int authorId, int width, int height, CancellationToken cancellationToken)
    {
        // create resized image
        using var resizedImage = image.Clone(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(width, height), Mode = ResizeMode.Crop
        }));

        // save as jpg
        await UploadToS3(resizedImage, authorId, width, height, "jpg", new JpegEncoder { Quality = 60 },
            cancellationToken);

        // save as webp
        await UploadToS3(resizedImage, authorId, width, height, "webp", new WebpEncoder { Quality = 60 },
            cancellationToken);
    }

    private async Task UploadToS3(
        Image image, int authorId, int width, int height, string extension, IImageEncoder encoder,
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

        // generate s3 key (e.g., "author-images/<author-id>-s.jpg")
        var fileName = $"{authorId}-{sizeSuffix}.{extension}";
        var key = $"author-images/{fileName}";

        // save image to a memory stream
        using var memoryStream = new MemoryStream();
        image.Save(memoryStream, encoder);
        memoryStream.Position = 0;

        // upload to s3
        var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(
            new TransferUtilityUploadRequest
            {
                InputStream = memoryStream, BucketName = _bucketName, Key = key, ContentType = $"image/{extension}"
            }, cancellationToken);
    }
}