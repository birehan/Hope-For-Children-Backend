using Domain;

namespace Application.Interfaces
{
       public interface IUserAccessor
    {
         string GetUsername();

         Task<AppUser>  GetCurrentUser();

    }
}