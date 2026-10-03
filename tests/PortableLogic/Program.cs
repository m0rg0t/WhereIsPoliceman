using System;
using System.Collections.Generic;
using System.Reflection;
using WhereIsPoliceman.ViewModel;
class Program { static void Main() { int count=0; foreach(var property in typeof(PolicemanItem).GetProperties()) { var item = new PolicemanItem(); var names = new List<string>(); item.PropertyChanged += (sender, args) => names.Add(args.PropertyName); object value = property.PropertyType == typeof(string) ? (object)"fixture" : property.PropertyType == typeof(bool) ? (object)true : Activator.CreateInstance(property.PropertyType); property.SetValue(item, value, null); if(!names.Contains(property.Name)) throw new Exception("Missing exact notification: " + property.Name + "; received " + string.Join(",", names)); if(!object.Equals(property.GetValue(item, null),value)) throw new Exception("Setter did not retain value: " + property.Name); count++; } Console.WriteLine("PASS: " + count + " property values and exact notification names"); } }
