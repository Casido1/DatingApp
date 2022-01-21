using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class UsersController : BaseAPIController
    {
        private readonly DataContext _cntx;
        public UsersController(DataContext cntx)
        {
            _cntx = cntx;
            
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            var users = _cntx.Users.ToList();
            return users;
        }

        [HttpGet("{id}")]
        public ActionResult<AppUser> GetUsers([FromRoute] int id)
        {
            return _cntx.Users.Find(id);
             
        }
    }
}