using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelancePlatform.Models;

namespace FreelancePlatform.Services
{
    public class RequestService
    {
        private readonly FreelancePlatformDbContext _context;
        public RequestService(FreelancePlatformDbContext context)
        {
            _context = context;
        }

        private bool RequestExist(Request request)
        {
            return _context.Requests.Any(r => r.Id == request.Id);
        }
        public bool SubmitRequest(Request request)
        {
            if (!RequestExist(request))
            {
                _context.Requests.Add(request);
                _context.SaveChanges();
                return true;
            }
            return false;

        }

        public List<Request> GetRequestsForProjects(int projectId)
        {
            var requests = _context.Requests.Where(r => r.ProjectId == projectId).ToList();
            return requests;
        }


    }
}
