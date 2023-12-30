using System.ComponentModel.DataAnnotations.Schema;
using Pgvector;

namespace Example.Model;

public class CloudResource
{
    public int Id { get; set; }

    public string Category { get; set; }

    public string Content { get; set; }

    /// <summary>
    /// A vector representing what is in the <see cref="Content"/> property.
    /// </summary>
    /// <remarks>
    /// Requires the Pgvector.EntityFrameworkCore NuGet package
    /// https://github.com/pgvector/pgvector-dotnet
    /// </remarks>
    [Column(TypeName = "vector(1536)")]
    public Vector? ContentVector { get; set; }

    public string Title { get; set; }

    /// <summary>
    /// A vector representing what is in the <see cref="Title"/> property.
    /// </summary>
    /// <remarks>
    /// Requires the Pgvector.EntityFrameworkCore NuGet package
    /// https://github.com/pgvector/pgvector-dotnet
    /// </remarks>
    [Column(TypeName = "vector(1536)")]
    public Vector? TitleVector { get; set; }

    public int VectorEmbeddingVersion { get; set; }
}