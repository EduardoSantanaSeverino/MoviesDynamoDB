﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DynamoDb.Libs.DynamoDb
{
    public class Item
    {
        public int Id { get; set; }
        public string ReplyDateTime { get; set; }
        public double Price { get; set; }
    }
}
