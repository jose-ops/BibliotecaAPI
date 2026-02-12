using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IS3Service
    {
        Task<string> UploadImagemLivroAsync(int id, IFormFile file);
    }
}
