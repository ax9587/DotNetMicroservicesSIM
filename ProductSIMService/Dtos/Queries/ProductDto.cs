using System;
using System.Collections.Generic;

namespace ProductSIMService.Dtos.Queries
{
    public class ProductDto
    {
        internal object questions;
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public IList<CoverDto> Covers { get; set; }        
        public IList<QuestionDto> Questions { get; set; }
        public int MaxNumberOfInsured { get; set; }
        
        public string Icon { get; set; }
    }
}
