using System;

namespace MyScullion.Features.Main
{

    public class MasterDetailPageMenuItem
    {
        public MasterDetailPageMenuItem(Type targetType)
        {
            TargetType = targetType;
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }        
    }
}