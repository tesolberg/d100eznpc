namespace D100EZNPC.Utilities
{
    public static class SkillCalculator
    {
        public static int VeryEasy(int skillVal) => skillVal * 2;
        public static int Easy(int skillVal) => (int)Math.Ceiling(skillVal * 1.5f);
        public static int Hard(int skillVal) => (int)Math.Ceiling(skillVal * 0.666667f);
        public static int Formidable(int skillVal) => (int)Math.Ceiling(skillVal * 0.5f);
        public static int Herculean(int skillVal) => (int)Math.Ceiling(skillVal * 0.1f);
    }
}
