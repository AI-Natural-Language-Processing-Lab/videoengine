using System;
using System.Linq;
using Jugnoon.Framework;
using Jugnoon.Utility;
using Microsoft.EntityFrameworkCore;
/// <summary>
/// Video Security Token Generator - Designed for Videos and Audio Files. 
/// It can be used with videos or audio files streaming via http handler (if module available)
/// </summary>
namespace Jugnoon.Videos
{
    public class TokenBLL
    {
        public static bool isTokenEnabled { get { return true; } }

        public static string Add(ApplicationDbContext context)
        {
            string Token = Guid.NewGuid().ToString();
            var _entity = new JGN_Tokens()
            {
                token = Token,
                created_at = DateTime.Now
            };

            context.Entry(_entity).State = EntityState.Added;
            context.SaveChanges();

            return Token.ToString();
        }

        public static bool Delete(ApplicationDbContext context, string Token)
        {
                var entity = new JGN_Tokens { token = Token };
                context.JGN_Tokens.Attach(entity);
                context.JGN_Tokens.Remove(entity);
                context.SaveChanges();
            

            return true;
        }

        public static bool Validate(ApplicationDbContext context, string Token)
        {
            bool flag = false;
            if (context.JGN_Tokens.Where(p => p.token == Token).Count() > 0)
                 flag = true;
            
            return flag;
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */

