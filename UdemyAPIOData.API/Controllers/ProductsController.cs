using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UdemyAPIOData.API.Models;

namespace UdemyAPIOData.API.Controllers
{
    // [ODataRoutePrefix("Products")]
    public class ProductsController : ODataController
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [EnableQuery(PageSize = 2)]
        public IActionResult GetProducts()

        {
            return Ok(_context.products.AsQueryable());
        }

        [ODataRoute("Products({Item})")]
        [EnableQuery]
        public IActionResult GetUrun([FromODataUri]int Item)
        {
            return Ok(_context.products.Where(x => x.Id == Item));
        }

        [ODataRoute("Products")]
        [HttpPost]
        public IActionResult Create([FromBody]Product product)
        {
            _context.products.Add(product);

            _context.SaveChanges();
            return Ok(product);
        }

        [ODataRoute("Products({Id})")]
        [HttpPut]
        public IActionResult Update([FromODataUri]int Id, [FromBody] Product product)

        {
            product.Id = Id;
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteProduct([FromODataUri]int key)
        {
            var product = _context.products.Find(key);

            if (product == null)
            {
                return NotFound();
            }
            _context.products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
    }
}