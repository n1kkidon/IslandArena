using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Skills
{
    public class SkillObject
    {
        public int SkillLevel { get; set; }
        public int SkillCap { get; set; }
        public string SkillName { get; set; }
        public string SkillDescription { get; set; }
        public bool Root { get; set; }
        public List<string> ChildSkillObjects { get; set; }
    }
}
