using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UdemyAPIOData.API.Models;

namespace UdemyAPIOData.API.Controllers
{
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IActionResult GetProducts()

        {
            return Ok(_context.products.AsQueryable());
        }

        [EnableQuery]
        public IActionResult Get([FromODataUri]int key)
        {
            return Ok(_context.products.Where(x => x.Id == key));
        }
    }
}