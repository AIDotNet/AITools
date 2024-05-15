using AI.Chat.Copilot.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Domain.Models
{
    public class AIApps
    {
        public int Id { get; set; }
        /// <summary>
        /// Name of the AI App
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of the AI App
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Prompt of the AI App
        /// </summary>
        public string? Prompt { get; set; }
        public int Temperature { get; set; } = 50;
        public int MaxTokens { get; set; } = 100;
        /// <summary>
        /// AI模型
        /// </summary>
        public AIModelType AIModelType { get; set; }
        /// <summary>
        /// 模型Id
        /// </summary>
        public string? ModelId { get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        public string? Secret { get; set; }
        /// <summary>
        /// 代理地址
        /// </summary>
        public string? ProxyHost { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTime { get; set; }
    }
}
