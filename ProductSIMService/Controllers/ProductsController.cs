using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductSIMService.Data;
using ProductSIMService.Dtos.Commands;
using ProductSIMService.Dtos.Queries;
using ProductSIMService.Model;
using ProductSIMService.Profiles;

namespace ProductSIMService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET api/products
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {

            Console.WriteLine("--> Getting Products....");

            var products = await _repository.FindAllActive();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            var result= products.Select(p => new ProductDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                MaxNumberOfInsured = p.MaxNumberOfInsured,
                Icon = p.ProductIcon,
                Questions = p.Questions != null ? ProductMapper.ToQuestionDtoList(p.Questions) : null,
                Covers = p.Covers.Any() ? ProductMapper.ToCoverDtoList(p.Covers) : null
            }).ToList();
            return Ok(result);
        }

        // GET api/products/{code}
        [HttpGet("{code}", Name = "GetByCode")]
        public async Task<ActionResult> GetByCode([FromRoute] string code)
        {

            var product = await _repository.FindOne(code);
            var productDto= product != null ? new ProductDto
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                MaxNumberOfInsured = product.MaxNumberOfInsured,
                Icon = product.ProductIcon,
                Questions = product.Questions != null ? ProductMapper.ToQuestionDtoList(product.Questions) : null,
                Covers = product.Covers != null ? ProductMapper.ToCoverDtoList(product.Covers) : null
            } : null;
            if (product != null)
            {
                return Ok(productDto);
            }

            return NotFound();
        }

        // POST api/products
        [HttpPost]
        public async Task<ActionResult> PostDraft([FromBody] ProductDraftDto request)
        {
            var draft = Product.CreateDraft
            (
                request.Code,
                request.Name,
                request.Image,
                request.Description,
                request.MaxNumberOfInsured,
                request.Icon
            );

            foreach (var cover in request.Covers)
            {
                draft.AddCover(cover.Code, cover.Name, cover.Description, cover.Optional, cover.SumInsured);
            }

            var questions = new List<Question>();
            //TODO https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/custom-formatters?view=aspnetcore-5.0
            //TODO https://www.tutorialdocs.com/article/webapi-data-binding.html
            foreach (var question in request.Questions)  //TODO add input options.InputFormatters.Insert(0, new VcardInputFormatter());
                                                             
            {
                switch (question)
                {
                    case Dtos.Commands.NumericQuestionDto numericQuestion:
                        questions.Add(new NumericQuestion(numericQuestion.QuestionCode, numericQuestion.Index,
                            numericQuestion.Text));
                        break;
                    case Dtos.Commands.DateQuestionDto dateQuestion:
                        questions.Add(new DateQuestion(dateQuestion.QuestionCode, dateQuestion.Index,
                            dateQuestion.Text));
                        break;
                    case Dtos.Commands.ChoiceQuestionDto choiceQuestion:
                        questions.Add(new ChoiceQuestion(choiceQuestion.QuestionCode, choiceQuestion.Index,
                            choiceQuestion.Text, choiceQuestion.Choices.Select(c => new Choice(c.Code, c.Label)).ToList()));
                        break;
                }
            }
            draft.AddQuestions(questions);

            await _repository.Add(draft);
            _repository.SaveChanges();
            var productDto = draft != null ? new ProductDto
            {
                Id = draft.Id,
                Code = draft.Code,
                Name = draft.Name,
                Description = draft.Description,
                Image = draft.Image,
                MaxNumberOfInsured = draft.MaxNumberOfInsured,
                Icon = draft.ProductIcon,
                Questions = draft.Questions != null ? ProductMapper.ToQuestionDtoList(draft.Questions) : null,
                Covers = draft.Covers != null ? ProductMapper.ToCoverDtoList(draft.Covers) : null
            } : null;
            return CreatedAtRoute(nameof(GetByCode), new { code = draft.Code }, productDto);
        }

        // POST api/products/activate
        [HttpPost("/activate")]
        public async Task<ActionResult> Activate([FromBody] ActivateProductCommandDto request)
        {

            var product = await _repository.FindById(request.ProductId);
            product.Activate();
            _repository.SaveChanges();
            var result= new ActivateProductResultDto
            {
                ProductId = product.Id
            };
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        // POST api/products/discontinue
        [HttpPost("/discontinue")]
        public async Task<ActionResult> Discontinue([FromBody] DiscontinueProductCommandDto request)
        {
            var product = await _repository.FindById(request.ProductId);
            product.Discontinue();
            _repository.SaveChanges();
            var result = new DiscontinueProductResultDto
            {
                ProductId = product.Id
            };
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

    }    
}
