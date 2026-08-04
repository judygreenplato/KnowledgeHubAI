using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeHub.Infrastructure.Integrations.Python;
public class PythonChatResponse
{
    public string Answer { get; set; } = string.Empty;

    public List<string> Sources { get; set; } = new();
}