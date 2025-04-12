using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FreelancePlatform.Models;


namespace FreelancePlatform.Services
{
    public class ProjectService
    {
        private readonly FreelancePlatformDbContext _context;
        public ProjectService(FreelancePlatformDbContext context)
        {
            _context = context;
        }
        public void CreateProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();

        }

        private bool CanBeUpdated(Project project)
        {
            var existingProject = _context.Projects.SingleOrDefault(u => u.Id == project.Id);

            return !string.IsNullOrEmpty(project.Name) &&
                project.Name != existingProject.Name &&
                !string.IsNullOrEmpty(project.Description)
                && project.Description != existingProject.Description;
        }
        public bool UpdateProject(Project project)
        {
            var existingProject = _context.Projects.SingleOrDefault(u => u.Id == project.Id);
            if (existingProject == null)
            {
                return false;
            }
            if (CanBeUpdated(project))
            {
                existingProject.Name = project.Name;
                existingProject.Description = project.Description;
                existingProject.Budget = project.Budget;
                existingProject.Status = project.Status;
                existingProject.Category = project.Category;
                existingProject.CustomerId = project.CustomerId;
                existingProject.ExecutorId = project.ExecutorId;

                return true;
            }
            return false;
        }

        public bool DeleteProject(int projectid)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == projectid);
            if (project == null)
            {
                return false;
            }
            _context.Projects.Remove(project);
            _context.SaveChanges();
            return true;
        }

        public List<Project> GetProjectByCustomer(int customerid)
        {
            var projects = _context.Projects.Where(p => p.CustomerId == customerid);
            return projects.ToList();
        }

        public bool AssignFreelancer(int projectid, int freelancerid)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == projectid);
            if (project == null)
            {
                return false;
            }
            var freelancer = _context.Users.FirstOrDefault(u => u.Id == freelancerid);
            if (freelancer == null)
            {
                return false;
            }
            project.Executor = freelancer;
            return true;
        }
    }
}
