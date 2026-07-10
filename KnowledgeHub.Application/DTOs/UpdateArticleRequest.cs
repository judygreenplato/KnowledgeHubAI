using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeHub.Application.DTOs;

    public class UpdateArticleRequest
    {
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }
}
