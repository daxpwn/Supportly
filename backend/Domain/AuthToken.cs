using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class AuthToken : BaseEntity
    {
        public string TokenId { get; set; } //Guid tokena
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? InvalidatedAt { get; set; }
        public int UserId { get; set; }
        public int? BaseTokenId { get; set; } //Ako je podatak null, u pitanu je JWT, ako ne onda je refresh token
        
        public virtual User User { get; set; }
        public virtual AuthToken JwtToken { get; set; }
        public virtual AuthToken RefreshToken { get; set; }
    }

    /*
        SELECT InvalidatedAt FROM AuthTokens a
        WHERE TokenId = a.TokenId
    */
}
