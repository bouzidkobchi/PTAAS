/*
 * POST /api/register       done
 * POST /api/login          done
 * POST /api/logout         done
 * POST /api/refresh-token  
 * POST /api/forgot-password    done
 * POST /api/reset-password     done
 * POST /api/change-password    done
 * GET /api/user                done
 * PUT /api/user                done
 * 
 * 
 * nots : the admin is a pentester by the way !
 */

/*
    problem of reading user roles from the  database , and the jwt  creation 
    
    refresh token
    
 */

namespace WebApi.Auth.Models
{
    public class ResetPasswordModel
    {
        public required string ResetToken { get; set; }
        public required string UserId { get; set; }
        public required string NewPassword { get; set; }
    }
}