using API.Models;
using API.Repositories;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace API.Controllers
{
    [Route("api/attendances")]
    [ApiController]
    public class AttendanceHistoriesController : ControllerBase
    {
        private readonly AttendanceHistoryRepository repository;

        public AttendanceHistoriesController(AttendanceHistoryRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("scan/{NIK}")]
        public virtual ActionResult ScanQR(string NIK)
        {
            var scan = repository.ScanQR(NIK);
            if (scan != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Berhasil Melakukan Absensi", Data = scan });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.NotFound, message = "Gagal Melakukan Absensi", Data = scan });
            }
        }

        [HttpGet("{NIK}")]
        public virtual ActionResult Get(string NIK)
        {
            var get = repository.GetByNIK(NIK);
            if (get.Count() != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = get.Count() + " Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = get.Count() + " Data Ditemukan", Data = get });
            }
        }

        [HttpGet("revise/{Id}")]
        public virtual ActionResult GetRevise(int Id)
        {
            var get = repository.GetRevise(Id);
            if (get.Count() != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = get.Count() + " Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = get.Count() + " Data Ditemukan", Data = get });
            }
        }

        [HttpPost("revise")]
        public virtual ActionResult Revise(AttendanceHistory attendanceHistory)
        {
            var revise = repository.Revise(attendanceHistory);
            if (revise >= 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Revisi Berhasil Diajukan", Data = revise });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Mengajukan Revisi", Data = revise });
            }
        }

        [HttpPost("response")]
        public virtual ActionResult ResponseRevise(AttendanceHistory attendanceHistory)
        {
            var response = repository.ResponseRevise(attendanceHistory);
            if (response >= 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Revisi Berhasil Ditanggapi", Data = response });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Menanggapi Revisi", Data = response });
            }
        }
    }
}
