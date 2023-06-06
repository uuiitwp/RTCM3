﻿namespace RTCM3.Common.Frequency
{
    public static class Frequency
    {
        public const double L1 = 1.57542E9;
        public const double L2 = 1.2276E9;
        public const double L5 = 1.17645E9;
        public const double L6 = 1.27875E9;
        public const double G1 = 1.602E9;
        public const double DG1 = 0.5625E6;
        public const double G2 = 1.24600E9;
        public const double DG2 = 0.4375E6;
        public const double G1a = 1.600995E9;
        public const double G2a = 1.248060E9;
        public const double G3 = 1.202025E9;
        public const double E1 = L1;
        public const double E5a = L5;
        public const double E5b = 1.20714E9;
        public const double E5ab = 1.191795E9;
        public const double E6 = L6;
        public const double B1 = 1.561098E9;
        public const double B1C = L1;
        public const double B1A = L1;
        public const double B2a = L5;
        public const double B2 = E5b;
        public const double B2b = E5b;
        public const double B2ab = E5ab;
        public const double B3 = 1.26852E9;
        public const double B3A = B3;
        private static readonly double[] msm_freq_gps = new double[32]{
            double.NaN,
            L1,L1,L1,double.NaN,double.NaN,double.NaN,
            L2,L2,L2,double.NaN,double.NaN,double.NaN,double.NaN,
            L2,L2,L2,double.NaN,double.NaN,double.NaN,double.NaN,
            L5,L5,L5,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,
            L1,L1,L1
        };
        private static readonly string[] msm_rinex_gps = new string[32]{
            "",
            "1C","1P","1W","","","",
            "2C","2P","2W","","","","",
            "2S","2L","2X","","","","",
            "5I","5Q","5X","","","","","",
            "1S","1L","1X"
        };
        private static readonly double[] msm_freq_glo = new double[32]{
            double.NaN,G1,G1,double.NaN,double.NaN,double.NaN,double.NaN,
            G2,G2,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,
            double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,
            double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,
            double.NaN,double.NaN,double.NaN
        };
        private static readonly string[] msm_rinex_glo = new string[32]{
            "","1C","1P","","","","",
            "2C","2P","","","","","",
            "","","","","","","",
            "","","","","","","","",
            "","",""
        };
        private static readonly double[] msm_freq_gal = new double[32]{
            double.NaN,E1,E1,E1,E1,E1,double.NaN,
            E6,E6,E6,E6,E6,double.NaN,
            E5b,E5b,E5b,double.NaN,E5ab,E5ab,E5ab,
            double.NaN,E5a,E5a,E5a,double.NaN,double.NaN,double.NaN,
            double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,
        };

        private static readonly string[] msm_rinex_gal = new string[32]{
            "","1C","1A","1B","1X","1Z","",
            "6C","6A","6B","6X","6Z","",
            "7I","7Q","7X","","8I","8Q","8X",
            "","5I","5Q","5X","","","",
            "","","","","",
        };

        private static readonly double[] msm_freq_sbas = new double[32]{
            double.NaN,L1,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,
            double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,
            double.NaN, double.NaN,double.NaN,double.NaN,double.NaN,
            double.NaN,double.NaN,double.NaN,double.NaN,L5,L5,L5,
            double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,
        };

        private static readonly string[] msm_rinex_sbas = new string[32]{
            "","1C","","","","","",
            "","","","","",
            "", "","","","",
            "","","","","5I","5Q","5X",
            "","","","","","","","",
        };


        private static readonly double[] msm_freq_qzss = new double[32]{
            double.NaN,L1,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,
            L6,L6,L6,double.NaN,double.NaN,double.NaN,L2,L2,L2,
            double.NaN,double.NaN,double.NaN,double.NaN,
            L5,L5,L5,double.NaN,double.NaN,double.NaN,
            double.NaN,double.NaN,L1,L1,L1
        };

        private static readonly string[] msm_rinex_qzss = new string[32]{
            "","1C","","","","","","",
            "6S","6L","6X","","","","2S","2L","2X",
            "","","","",
            "5I","5Q","5X","","","",
            "","","1S","1L","1X"
        };


        private static readonly double[] msm_freq_bds = new double[32]{
            double.NaN,B1,B1,B1,double.NaN,double.NaN,double.NaN,
            B3,B3,B3,double.NaN,double.NaN,double.NaN,
            B2,B2,B2,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,
            B2a,B2a,B2a,B2b,double.NaN,double.NaN,double.NaN,double.NaN,
            B1C,B1C,B1C,
        };

        private static readonly string[] msm_rinex_bds = new string[32]{
            "","2I","2Q","2X","","","",
            "6I","6Q","6X","","","",
            "7I","7Q","7X","","","","","",
            "5D","5P","5X","7D","","","","",
            "1D","1P","1X",
        };


        private static readonly double[] glonass_frequency_number = new double[32]{
            1,-4,5,6,1,-4,5,6,-2,-7,0,-1,-2,-7,0,-1,4,-3,3,2,4,-3,3,2,
            double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN
        };

        public static double[] GlonassFrequencyNumber
        {
            get => glonass_frequency_number;
        }

        public static double GetGlonassFrequency(int satId, int sigId)
        {
            double f = msm_freq_glo[sigId - 1];
            return f switch
            {
                G1 => f + DG1 * glonass_frequency_number[satId - 1],
                G2 => f + DG2 * glonass_frequency_number[satId - 1],
                _ => double.NaN,
            };
        }

        public static string GetRinexCode(GNSSSystem GNSSSystem, int sigId)
        {
            return GNSSSystem switch
            {
                GNSSSystem.GPS => msm_rinex_gps[sigId - 1],
                GNSSSystem.GLONASS => msm_rinex_glo[sigId - 1],
                GNSSSystem.SBAS => msm_rinex_sbas[sigId - 1],
                GNSSSystem.GALILEO => msm_rinex_gal[sigId - 1],
                GNSSSystem.QZSS => msm_rinex_qzss[sigId - 1],
                GNSSSystem.BEIDOU => msm_rinex_bds[sigId - 1],
                _ => "",
            };
        }

        public static double GetFrequency(GNSSSystem GNSSSystem, int satId, int sigId)
        {
            return GNSSSystem switch
            {
                GNSSSystem.GPS => msm_freq_gps[sigId - 1],
                GNSSSystem.GLONASS => GetGlonassFrequency(satId, sigId),
                GNSSSystem.SBAS => msm_freq_sbas[sigId - 1],
                GNSSSystem.GALILEO => msm_freq_gal[sigId - 1],
                GNSSSystem.QZSS => msm_freq_qzss[sigId - 1],
                GNSSSystem.BEIDOU => msm_freq_bds[sigId - 1],
                _ => double.NaN,
            };

        }
    }

}
