/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
using System;
using System.Collections;
using System.Collections.Generic;

namespace ItinerariesApi.Models
{

	/// 
	/// <summary>
	/// @author Ajuz Adolfo
	/// @since 22/Oct/2016
	/// </summary>
	public class OptimizedItinerary : List<ScoredAttraction>
	{

		private double TotalDistance;
		private double TotalScore;

		public OptimizedItinerary()
		{
		}

		public OptimizedItinerary(OptimizedItinerary itinerario)
		{
		   this.TotalDistance = itinerario.TotalDistance;
		   this.TotalScore = itinerario.TotalScore;

		   base.Clear();
            
		   //super.addAll(itinerario.subList(0, itinerario.size() - 1));
		   base.AddRange(itinerario.GetRange(0, itinerario.Count));
           

		   //
		}

		public virtual double Distance
		{
			get
			{
				return TotalDistance;
			}
			set
			{
				this.TotalDistance = value;
			}
		}


		public virtual double Score
		{
			get
			{
				return TotalScore;
			}
			set
			{
				this.TotalScore = value;
			}
		}

	}

}