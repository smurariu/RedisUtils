﻿namespace Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Website { get; set; }

        public override string ToString()
        {
            return $"Id: [{Id}], Name: [{Name}], Website: [{Website}]";
        }
    }
}
