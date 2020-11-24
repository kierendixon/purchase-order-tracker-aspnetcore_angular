﻿using System;
using System.Data.SqlClient;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace PurchaseOrderTracker.Persistence.Initialization
{
    public static class DbInitializerHelper
    {
        public static bool EnsureDatabaseCreated(DbContext context)
        {
            while (true)
            {
                try
                {
                    return context.Database.EnsureCreated();
                }
                catch (SqlException ex)
                {
                    if (ex.Number != 53 && ex.Number != 18456)
                    {
                        throw;
                    }

                    Console.WriteLine($"Could not connect to database. {ex.Message}");
                    Console.WriteLine("Retrying after 3 seconds...");
                    Thread.Sleep(3000);
                }
            }
        }
    }
}
