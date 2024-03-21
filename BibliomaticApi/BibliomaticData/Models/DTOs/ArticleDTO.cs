using System;
using System.Collections.Generic;
namespace BibliomaticData.Models.DTOs
{
    public class ArticleDTO : BaseTrackedEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }       
    }
}
