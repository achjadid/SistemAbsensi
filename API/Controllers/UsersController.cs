using API.Models;
using API.Repositories;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository repository;

        public UsersController(UserRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public virtual ActionResult Get()
        {
            var get = repository.GetAllUserEmployee();
            if (get.Count() != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = get.Count() + " Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.NotFound, message = get.Count() + " Data Ditemukan", Data = get });
            }
        }

        [HttpGet("Department-{id}")]
        public virtual ActionResult GetByDepartment(int id)
        {
            var get = repository.GetUserEmployeeByDepartment(id);
            if (get.Count() != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = get.Count() + " Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.NotFound, message = get.Count() + " Data Ditemukan", Data = get });
            }
        }

        [HttpGet("{NIK}")]
        public virtual ActionResult GetByNIK(string NIK)
        {
            var get = repository.GetUserEmployeeByNIK(NIK);
            if (get != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Ditemukan", Data = get });
            }
        }

        [HttpPost]
        public virtual ActionResult Insert(UserEmployeeVM userEmployeeVM)
        {
            var insert = repository.InsertUserEmployee(userEmployeeVM);
            if (insert >= 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil Dimasukkan", Data = insert });
            }
            else if (insert == -11)
            {
                return StatusCode(500, new { status = HttpStatusCode.OK, message = "Gagal Memasukkan Data. Username sudah digunakan.", Data = insert });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Memasukkan Data", Data = insert });
            }
        }

        [HttpPut]
        public virtual ActionResult Update(UserEmployeeVM userEmployeeVM)
        {
            var update = repository.UpdateUserEmployee(userEmployeeVM);
            if (update >= 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil Diperbaharui", Data = update });
            }
            else if (update == -11)
            {
                return StatusCode(500, new { status = HttpStatusCode.OK, message = "Gagal Memperbaharui Data. Username sudah digunakan.", Data = update });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Memperbaharui Data", Data = update });
            }
        }

        [HttpDelete("{NIK}")]
        public ActionResult Delete(string NIK)
        {
            var delete = repository.Delete(NIK);
            if (delete >= 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil Dihapus", Data = delete });
            }
            else if (delete == 0)
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data dengan NIK " + NIK + "Tidak Ditemukan", Data = delete });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Terjadi Kesalahan", Data = delete });
            }
        }

        [HttpPut("reset-password")]
        public virtual ActionResult ResetPassword(User user)
        {
            var update = repository.UpdatePassword(user, true);
            if (update >= 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Password Berhasil Direset", Data = update });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Mereset Password", Data = update });
            }
        }

        [HttpPut("update-password")]
        public virtual ActionResult UpdatePassword(User user)
        {
            var update = repository.UpdatePassword(user);
            if (update >= 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Password Berhasil Diperbaharui", Data = update });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Memperbaharui Password", Data = update });
            }
        }
    }
}
