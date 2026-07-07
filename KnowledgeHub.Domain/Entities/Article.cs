using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeHub.Domain.Entities;
public class Article
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public bool IsPublished { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
    public Guid CategoryId { get; set; }

    public Category Category { get; set; } = null!;
}
