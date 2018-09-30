using ItinerariesApi.Models;
using System;
using System.Collections.Generic;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace ItinerariesApi.Models
{

    public class ScoredAttraction : ICloneable
	{

        public ScoredAttraction()
        {

        }

		public virtual int Position { get; set; }
        public virtual int ExternalId { get; set; }
        public virtual int ExternalCategoryId { get; set; }

        public virtual bool WasVisited { get; set; }


		public virtual double ArtScore { get; set; }


		public virtual double HistoryScore { get; set; }


        public virtual double ReligionScore { get; set; }


        public virtual double NatureScore { get; set; }


        public static double CalculateArtScore(BelongingScoreCategory belongingScoreCategory, double referenceValue)
        {
            double multiplier = 1;
            switch (belongingScoreCategory)
            {
                case BelongingScoreCategory.Art:
                    multiplier = 2;
                    break;
                case BelongingScoreCategory.History:
                    multiplier = 1;
                    break;
                case BelongingScoreCategory.Religion:
                    multiplier = 0.5;
                    break;
                case BelongingScoreCategory.Nature:
                    multiplier = 0.5;
                    break;

            }
            return multiplier * referenceValue;
        }
        public static double CalculateHistoryScore(BelongingScoreCategory belongingScoreCategory, double referenceValue)
        {
            double multiplier = 1;
            switch (belongingScoreCategory)
            {
                case BelongingScoreCategory.Art:
                    multiplier = 1;
                    break;
                case BelongingScoreCategory.History:
                    multiplier = 2;
                    break;
                case BelongingScoreCategory.Religion:
                    multiplier = 0.5;
                    break;
                case BelongingScoreCategory.Nature:
                    multiplier = 0.5;
                    break;

            }
            return multiplier * referenceValue;
        }
        public static double CalculateReligionScore(BelongingScoreCategory belongingScoreCategory, double referenceValue)
        {
            double multiplier = 1;
            switch (belongingScoreCategory)
            {
                case BelongingScoreCategory.Art:
                    multiplier = 1;
                    break;
                case BelongingScoreCategory.History:
                    multiplier = 1;
                    break;
                case BelongingScoreCategory.Religion:
                    multiplier = 2;
                    break;
                case BelongingScoreCategory.Nature:
                    multiplier = 0.5;
                    break;

            }
            return multiplier * referenceValue;
        }
        public static double CalculateNatureScore(BelongingScoreCategory belongingScoreCategory, double referenceValue)
        {
            double multiplier = 1;
            switch (belongingScoreCategory)
            {
                case BelongingScoreCategory.Art:
                    multiplier = 1;
                    break;
                case BelongingScoreCategory.History:
                    multiplier = 1;
                    break;
                case BelongingScoreCategory.Religion:
                    multiplier = 0.5;
                    break;
                case BelongingScoreCategory.Nature:
                    multiplier = 2;
                    break;

            }
            return multiplier * referenceValue;
        }

        public object Clone()
        {
            try
            {
                return (ScoredAttraction)base.MemberwiseClone();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new Exception();
            }
        }
    }

}