
using System;
using System.Collections.Generic;
using System.Linq;

namespace ItinerariesApi.Models
{

	/// <summary>
	/// 
	/// Traducción a c# realizada a partir de la implementación en Java  por Msc. Adolfo Urrutia Zambrana
	/// @date: September, 2018
	/// </summary>
	public class CustomVNSItinerary
	{

		private  double[] touristWeights;
		private double[][] Distances;
		private double kExponent;
		private List<ScoredAttraction> filteredAttractions; 
		private int originAttractionPosition;

        public CustomVNSItinerary(double[] touristWeights, double[][] matrizDistancias, List<ScoredAttraction> attractions, int posOrigen, double k = 5)
        {
            this.touristWeights = touristWeights;
            Distances = matrizDistancias;
            kExponent = k;
            originAttractionPosition = posOrigen;
            this.filteredAttractions = attractions;
            this.filteredAttractions[posOrigen].WasVisited = true;

        }
		public  OptimizedItinerary CreateSingleDayItinerary(double maxDistance, int turns)
		{
			OptimizedItinerary optimalRoute = new OptimizedItinerary();
			OptimizedItinerary localItinerary = new OptimizedItinerary();

            //Para qué se usa estas dos variables con tamaño 2
			double[] dblDistScore;
			double[] dblDistScoreTemp;
			dblDistScore = new double[2];
			dblDistScoreTemp = new double[2];

			double dblScoreOptimo = 0.0;
			localItinerary = GetSimpleGreedySolution(filteredAttractions[this.originAttractionPosition], maxDistance);

			for (int i = 0; i < turns; i++)
			{

				localItinerary = PerformVnd(localItinerary);

				//Perturbación
				localItinerary = RandomInsertion(localItinerary, maxDistance, filteredAttractions);
                
                //Local Search
				do
				{
					do
					{
						dblDistScore = dblDistScoreTemp;
						localItinerary = PerformVnd(localItinerary);
						localItinerary = InsertBestPosition(localItinerary, maxDistance, filteredAttractions);

						dblDistScoreTemp = GetDistanceScore(localItinerary);

					} while (dblDistScoreTemp[1] > dblDistScore[1]);

					localItinerary = Remove2Insert1(localItinerary, maxDistance, filteredAttractions);
					dblDistScoreTemp = GetDistanceScore(localItinerary);

				} while (dblDistScoreTemp[1] > dblDistScore[1]);

				dblDistScoreTemp[0] = 0;
				dblDistScoreTemp[1] = 0;

				dblDistScoreTemp = GetDistanceScore(localItinerary);

				if (dblDistScoreTemp[1] > dblScoreOptimo && dblDistScoreTemp[0] <= maxDistance)
				{
					optimalRoute.Clear();
					optimalRoute.AddRange(localItinerary);
					dblScoreOptimo = dblDistScoreTemp[1];
				}
                //En este caso que el itinerario para remover tiene que tener al menos 3 elementos para poder remover aleatoriamente
                if (localItinerary.Count >= 3 && i < turns - 1 && localItinerary.Count < filteredAttractions.Count)//se agrega la validación de i < turns -1 a fin de no remover si se sabe que se está en la última iteración de la metaheurística
                {
                    //Si está en la primera mitad del bucle remueve aleatoriamente solo una vez
                    if (i < turns / 2)
                    {
                        localItinerary = RandomRemoval(localItinerary, filteredAttractions);
                    }
                    else
                    {
                        localItinerary = RandomRemoval(localItinerary, filteredAttractions);
                        if (localItinerary.Count >= 3)
                        {
                            localItinerary = RandomRemoval(localItinerary, filteredAttractions);
                        }

                    }
                }//Fin del if de la longitud del itinerario
                else {
                    //Si ya se incluyó todas las atracciones del itinerario significa que ya no hay nada que mejorar más y por tanto como ya no se aplicará las perturbaciones
                    //entonces se aplica el break al ciclo
                    break;
                } 
			} //Fin del For

			//dblDistScoreTemp = getDistanciaScoreItinerarioNew(objRutaOptima);
			dblDistScoreTemp = GetDistanceScore(optimalRoute);

			optimalRoute.Distance = dblDistScoreTemp[0];
			optimalRoute.Score = dblDistScoreTemp[1];

			return optimalRoute;
		}

		/// <summary>
		/// El Greedy anterior se quedaba atrapado en un mínimo local, porque no
		/// creaba una lista de entidades ordenadas por el Score, sino que siempre
		/// iba a la de mayor Score que no había sido visitada.
		/// </summary>
		/// <param name="dblK"> </param>
		/// <param name="intW"> </param>
		/// <param name="dblLimiteFlex">
		/// @return </param>
		public  OptimizedItinerary GetSimpleGreedySolution(ScoredAttraction inicialScoredAttraction, double limitDistance)
		{
			double dblScore = 0.0;
			double dblDistancia = 0.0;
            //En este array score distancia temporal en el [0] se guarda el score, y en [1] se guarda la distancia
			double[] dblScoreDistTemp = new double[2];

			bool hayCandidato = true;
            //Entero de la posicion inicial potencial de la atraccion
			int intPotencialEcpPos = -1;

			//Selecciono el nodo inicial
			List<ScoredAttraction> alExcluidasGreedy = new List<ScoredAttraction>();
			HashSet<int> hsExcluidasGreedy = new HashSet<int>();

			OptimizedItinerary objItinerarioGreedy = new OptimizedItinerary();

			//Agregando la entidad desde la cual inicia
			objItinerarioGreedy.Add(inicialScoredAttraction);

			//Y como finaliza donde empieza también lo agrego
			objItinerarioGreedy.Add(inicialScoredAttraction);

			//Excluyendo de la búsqueda a la atraccion inicial
			alExcluidasGreedy.Add(inicialScoredAttraction);
			hsExcluidasGreedy.Add(inicialScoredAttraction.Position);

			//Mientras no nos gastemos el tiempo disponible, incluyendo ya la distancia al nodo final.
			while (hayCandidato)
			{
				hayCandidato = false;

				//Recorro todas las atracciones filtradas de forma iterativa y ordenada desde la posición 1 hasta count, aca tengo que consultar si pq la inicial esta en indice cero
				for (int i = 1; i < filteredAttractions.Count; i++)
				{
                    //Si no ha sido visitada
					if (!hsExcluidasGreedy.Contains(filteredAttractions[i].Position))
					{
                        //Se aplica ese calculo para obtener un array de score-distancia pasando el itinerario actual del greedy, y la atraccion que se va a evaluar su candidatura
						dblScoreDistTemp = CalculatePotentialScoreDistanceByInsertion(objItinerarioGreedy, filteredAttractions[i], objItinerarioGreedy.Count - 1);

						//Busco al que tenga el mayor Score en cada iterarion, evaluando si hay mejoria respecto a lo que se ha iterado antes y si la nueva distancia temporal no viola el limite
						if (dblScoreDistTemp[0] > dblScore && dblScoreDistTemp[1] <= limitDistance)
						{
                            //para apuntar al mayor el score para la siguiente iteracion
							dblScore = dblScoreDistTemp[0];
                            //se almacena la distancia de ese candidato
							dblDistancia = dblScoreDistTemp[1];
                            //se almacena la posicion del candidato
							intPotencialEcpPos = i;
							hayCandidato = true;
						}
					}
				} //Fin del for

				//Una vez ya conocida la Entidad que le viene mejor habrá que ver si no sobrepasamos la restricción del tiempo
				if (hayCandidato)
				{
                    //Se hace un add o más bien insert al final del itinerario greedy, 
					objItinerarioGreedy.Insert(objItinerarioGreedy.Count - 1, filteredAttractions[intPotencialEcpPos]);

                    //Marcando la atracción como visitada
                    filteredAttractions[intPotencialEcpPos].WasVisited = true;

					//Si el límite de distancia hace imposible insertar, entonces debe ser excluida de su posicion en la matriz         
					hsExcluidasGreedy.Add(filteredAttractions[intPotencialEcpPos].Position);
                    //Se calcula la distancia de un itinerario con el objeto greedy nuevo
					dblDistancia = CalculateItineraryDistance(objItinerarioGreedy);
					dblScore = 0.0;
				}

			} //Fin del While

			return objItinerarioGreedy;
		}

		/// <summary>
		/// Combina los metodos 2-opt, 3-opt y mejor posición para obtener un mínimo
		/// local respecto a todos ellos.
		/// 
		/// <param name="route">
		/// @return </param>
        /// //Aca veo no se usa la ruta mejorada2
		public  OptimizedItinerary PerformVnd(OptimizedItinerary route)
		{

			OptimizedItinerary objRutaMejorada = new OptimizedItinerary();
			//Itinerario objRutaMejorada2 = new Itinerario(); Comentado pq no se usa aca
            //Consultar que simboliza los intentos del algoritmo
			int intentos = 2;
			double? dblDist;
			double? dblDistOpt;

			objRutaMejorada = new OptimizedItinerary(route);
			//objRutaMejorada2 = new Itinerario(objRuta);
			dblDist = CalculateItineraryDistance(route);

			dblDistOpt = dblDist;
            //La idea aca es salir cuando la distancia optima sea mayor que la de referencia
			do
			{
				do
				{
					do
					{
                        //se guarda la actual
						dblDist = dblDistOpt;
                      
                        //se mejora la ruta y se actualiza en este objeto de forma que hasta que deje de haber mejora, ya en el siguiente bucle se tiene el objRuta mas mejorada
						objRutaMejorada = Perform2Opt(objRutaMejorada, intentos);
                        //se obtiene la distancia optimizada actualizada de forma que se compara si se logró reducir respecto a la anterior
                        dblDistOpt = CalculateItineraryDistance(objRutaMejorada);
					} while (dblDistOpt < dblDist);

					objRutaMejorada = Get3Opt(objRutaMejorada, intentos);

					dblDistOpt = objRutaMejorada.Distance;
				} while (dblDistOpt < dblDist);
				objRutaMejorada = MoveToBestPosition(objRutaMejorada);

				dblDistOpt = objRutaMejorada.Distance;
			} while (dblDistOpt < dblDist);

			return objRutaMejorada;

		}

		/// <summary>
		/// Mejora la velocidad del 2-Opt haciendo una comparación de los arcos
		/// extraidos y reconfigurados, antes de hacer el verdadero cambio
		/// 
		/// <param name="itinerary"> </param>
		/// <param name="cycles">
		/// @return </param>
		private  OptimizedItinerary Perform2Opt(OptimizedItinerary itinerary, int cycles)
		{
			OptimizedItinerary itinerario2optFinal = new OptimizedItinerary();

			int size = itinerary.Count;
			int improve = 0;
			int idImenos1 = -1;
			int idI = -1;
			int idKmas1 = -1;
			int idK = -1;

			double iMenosaI;
			double kaKmas1;

			double iaKmas1;
			double iMenos1aK;
			double iaK;

			itinerario2optFinal = itinerary;

			while (improve < cycles)
			{

				//Comienza desde el primer vértice y llega hasta el penúltimo
				for (int i = 1; i < size - 2; i++)
				{

					idImenos1 = itinerario2optFinal[i - 1].Position;
					idI = itinerario2optFinal[i].Position;

					//Comienza desde el segundo vértice y llega hasta el final
					for (int k = i + 2; k < size - 2; k++)
					{

                        //Se hace una verificación de cruces entre distancias
						idK = itinerario2optFinal[k].Position;
						idKmas1 = itinerario2optFinal[k + 1].Position;
                        //Estas no se usaron en la comparación
						iMenosaI = Distances[idImenos1][idI];
						kaKmas1 = Distances[idK][idKmas1];
						iaKmas1 = Distances[idI][idKmas1];
						iMenos1aK = Distances[idImenos1][idK];
						iaK = Distances[idI][idK];
                        //Se comparan los adyacentes de i-1
                        
						if ((Distances[idImenos1][idK] + Distances[idI][idKmas1]) < (Distances[idImenos1][idI] + Distances[idK][idKmas1]))
						{

							itinerario2optFinal = Perform2OptSwap(itinerario2optFinal, i, k);

						}

					}
				}
				improve++;
			}

			return itinerario2optFinal;
		}

		
		private static OptimizedItinerary Perform2OptSwap(OptimizedItinerary itinerario, int i, int k)
		{

			OptimizedItinerary itinerario2opt = new OptimizedItinerary();

			int size = itinerario.Count;

			// 1. take route[0] to route[i-1] and add them in order to new_route
			for (int c = 0; c <= i - 1; ++c)
			{
				itinerario2opt.Insert(c, itinerario[c]);
			}

			// 2. take route[i] to route[k] and add them in reverse order to new_route
			int dec = 0;
			for (int c = i; c <= k; ++c)
			{
				itinerario2opt.Insert(c, itinerario[k - dec]);
				dec++;
			}

			// 3. take route[k+1] to end and add them in order to new_route
			for (int c = k + 1; c < size; ++c)
			{
				itinerario2opt.Insert(c, itinerario[c]);
			}

			return itinerario2opt;
		}

		/// <summary>
		/// Ofrece una forma más rápida de aplicar el 3-Opt a un itinerario, por
		/// medio de la comparación previa de las 4 Configuraciones de las aristas
		/// posibles, si se detecta una reducción de la distancia, entonces es que se
		/// aplica El verdadero 3-Opt
		/// 
		/// <param name="itinerary"> </param>
		/// <param name="cycles">
		/// @return </param>
		private  OptimizedItinerary Get3Opt(OptimizedItinerary itinerary, int cycles)
		{

			OptimizedItinerary itinerario3optTemp = new OptimizedItinerary();
			OptimizedItinerary itinerario3optFinal = new OptimizedItinerary();

			int size = itinerary.Count;
			int improve = 0;
			int intCaso = -1;
			double best_distance = 0.0;
			double new_distance = 0.0;
			double dblDistBase = 0.0;
			double dblCaso1 = 0.0;

			best_distance = CalculateItineraryDistance(itinerary);
			new_distance = best_distance;
			itinerario3optFinal = itinerary;

			while (improve < cycles)
			{

				//Comienza desde el primer vértice y llega hasta el penúltimo
				for (int i = 1; i < size - 3; i++)
				{
					for (int j = i + 1; j < size - 2; j++)
					{
						for (int k = j + 1; k < size - 1; k++)
						{

							dblDistBase = (Distances[itinerario3optFinal[i].Position][itinerario3optFinal[i + 1].Position]) + (Distances[itinerario3optFinal[j].Position][itinerario3optFinal[j + 1].Position]) + (Distances[itinerario3optFinal[k].Position][itinerario3optFinal[k + 1].Position]);

							//Probando el caso 1-----------------------------
							dblCaso1 = (Distances[itinerario3optFinal[i].Position][itinerario3optFinal[j + 1].Position]) + (Distances[itinerario3optFinal[k].Position][itinerario3optFinal[i + 1].Position]) + (Distances[itinerario3optFinal[j].Position][itinerario3optFinal[k + 1].Position]);

							if (dblDistBase > dblCaso1)
							{
								intCaso = 1;
							}
							else if (dblDistBase > (Distances[itinerario3optFinal[i].Position][itinerario3optFinal[j + 1].Position]) + (Distances[itinerario3optFinal[k].Position][itinerario3optFinal[j].Position]) + (Distances[itinerario3optFinal[i + 1].Position][itinerario3optFinal[k + 1].Position]))
							{
								intCaso = 2;
							}
							else if (dblDistBase > (Distances[itinerario3optFinal[i].Position][itinerario3optFinal[k].Position]) + (Distances[itinerario3optFinal[j + 1].Position][itinerario3optFinal[i + 1].Position]) + (Distances[itinerario3optFinal[j].Position][itinerario3optFinal[k + 1].Position]))
							{
								intCaso = 3;
							}
							else if (dblDistBase > (Distances[itinerario3optFinal[i].Position][itinerario3optFinal[j].Position]) + (Distances[itinerario3optFinal[i + 1].Position][itinerario3optFinal[k].Position]) + (Distances[itinerario3optFinal[j + 1].Position][itinerario3optFinal[k + 1].Position]))
							{
								intCaso = 4;
							}

							if (intCaso != -1)
							{

								itinerario3optTemp = Perform3OptSwap(itinerario3optFinal, i, j, k, intCaso);
								new_distance = CalculateItineraryDistance(itinerario3optTemp);
                                //Para continuar el proceso si la nueva distancia es menor que la mejor
								if (best_distance > new_distance)
								{
									improve = 0;
									itinerario3optFinal = itinerario3optTemp;
									best_distance = new_distance;
								}
								else
								{
									//System.out.println("Caso malo: " + intCaso);
								}
								intCaso = -1;
							}
						} //For de la k
					} //For de la j
				} //For de la j
				improve++;
			}

			itinerario3optFinal.Distance = best_distance;
			return itinerario3optFinal;
		}

		/// <summary>
		/// Devolver un itinerario al que se le aplicó un 3 opt
		/// 
		/// <param name="itineraries"> </param>
		/// <param name="i"> </param>
		/// <param name="j"> </param>
		/// <param name="k">
		/// @return </param>
		private static OptimizedItinerary Perform3OptSwap(OptimizedItinerary itineraries, int i, int j, int k, int caseInt)
		{

			OptimizedItinerary itinerario3opt = new OptimizedItinerary();
			int size = itineraries.Count;

			switch (caseInt)
			{
				case 1:

					/* Caso 1------------------------------------------------:
					*  j->j+1
					*  k->j+1
					*  j->k+1
					 */
					//Insertando desde el inicio hasta i ----
					for (int z = 0; z <= i; z++)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					//Insertando desde j+1 hasta k-----
					for (int z = j + 1; z <= k; z++)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					//Insertando desde i+1 hasta j----
					for (int z = i + 1; z <= j; z++)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					//Insertando desde k+1 hasta el final.
					for (int z = k + 1; z < size; z++)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					break;
				case 2:

					/* Caso 2------------------------------------------------:
					*  j->j+1
					*  k->j+1
					*  j->k+1
					 */
					//Insertando desde el inicio hasta i ----
					for (int z = 0; z <= i; z++)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					//Insertando desde j+1 hasta k-----
					for (int z = j + 1; z <= k; z++)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					//Insertando desde j hasta i+1----
					for (int z = j; z >= i + 1; z--)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					//Insertando desde k+1 hasta el final.
					for (int z = k + 1; z < size; z++)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					break;
				case 3:

					/* Caso 3------------------------------------------------:
					*  i->k
					*  j+1->j+1
					*  j->k+1
					 */
					//Insertando desde el inicio hasta i ----
					for (int z = 0; z <= i; z++)
					{
						itinerario3opt.Insert(z, itineraries[z]);
					}

					//Insertando desde k hasta j+1-----
					for (int z = k; z >= j + 1; z--)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					//Insertando desde i+1 hasta j
					for (int z = i + 1; z <= j; z++)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					//Insertando desde k+1 hasta el final.
					for (int z = k + 1; z < size; z++)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					break;
				case 4:

					/* Caso 4------------------------------------------------:
					*  i->j
					*  i+1->k
					*  j+1->k+1
					 */
					//Insertando desde el inicio hasta j ----
					for (int z = 0; z <= i; z++)
					{
						itinerario3opt.Insert(z, itineraries[z]);
					}

					//Insertando desde la j hasta la j+1
					for (int z = j; z >= i + 1; z--)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					//Insertando desde la k hasta la j+1
					for (int z = k; z >= j + 1; z--)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					//Insertando desde k+1 hasta el final.
					for (int z = k + 1; z < size; z++)
					{
						itinerario3opt.Add(itineraries[z]);
					}

					break;
			}

			return itinerario3opt;
		}

		/// <summary>
		/// Inserta una atracción elegida de forma aletoria en el itinerario
		/// 
		/// <param name="itinerary">
		/// @return </param>
		private  OptimizedItinerary RandomInsertion(OptimizedItinerary itinerary, double limitDistance, List<ScoredAttraction> scoredAttractions)
		{

			OptimizedItinerary itinerarioInsertado = new OptimizedItinerary();

			double[] dblScoreDistPotencial = new double[] {0.0, limitDistance};
			double[] dblScoreDistPotencialNew = new double[] {0.0, limitDistance};

			double[] dblScoreDistFinal = new double[] {0.0, 0.0};

			int intPosicion = 1;
			int intAttraccion = 1;
			int VueltasWhile = 0;
			bool blHayCandidato = false;

			itinerarioInsertado = itinerary;

			Random r = new Random();
            //Para evitar tomar en cuenta al primero y al ultimo
			intAttraccion = r.Next(scoredAttractions.Count - 1) + 1;
            //Si hay mas de dos puntos entonces genera el random sino usa la posicion 1 es decir de segunda
			if (itinerarioInsertado.Count > 2)
			{
				intPosicion = r.Next(itinerarioInsertado.Count - 2) + 1;
			}
			else
			{
				intPosicion = 1;
			}

			//Recorro cada una de las atracciones candidatas
			while (VueltasWhile < scoredAttractions.Count)
			{

				if (scoredAttractions[intAttraccion].WasVisited == true)
				{
                    //Una atracción aleatoria del conjunto de atracciones
					intAttraccion = r.Next(scoredAttractions.Count - 1) + 1;

					if (itinerarioInsertado.Count > 2)
					{
						intPosicion = r.Next(itinerarioInsertado.Count - 2) + 1;
					}
					else
					{
						intPosicion = 1;
					}
				}
				else
				{
                    //Si no ha sido visitada entonces se calcula el impacto de una insercion potencial en una posicion aleatoria.
					dblScoreDistPotencial = CalculatePotentialScoreDistanceByInsertion(itinerarioInsertado, scoredAttractions[intAttraccion], intPosicion);
                    //Si la distancia final es mayor al limite se genera otro aleatorio para la atraccion y otra posición dentro del itinerario
					if (dblScoreDistPotencial[1] > limitDistance)
					{

						intAttraccion = r.Next(scoredAttractions.Count - 1) + 1;

						if (itinerarioInsertado.Count > 2)
						{
							intPosicion = r.Next(itinerarioInsertado.Count - 2) + 1;
						}
						else
						{
							intPosicion = 1;
						}
					}
					else
					{
						if (scoredAttractions[intAttraccion].WasVisited == false)
						{
							blHayCandidato = true;
							dblScoreDistFinal = dblScoreDistPotencial;
							break;
						}
					}
				}

				VueltasWhile++;
			} //Fin del While

            //Si se encuentra al primer candidato aleatorio entonces se inserta en la posicion usada de ultimo y se actualiza el score y distancia de dicho itinerario
			if (blHayCandidato)
			{
				itinerarioInsertado.Insert(intPosicion, scoredAttractions[intAttraccion]);
				itinerarioInsertado.Score = dblScoreDistFinal[0];
				itinerarioInsertado.Distance = dblScoreDistFinal[1];

				scoredAttractions[intAttraccion].WasVisited = true;

			}

			return itinerarioInsertado;
		}

        /// <summary>
        /// Recorre cada una de las atracciones no visitadas y por cada una prueba el
        /// mejor sitio, y si supera el score actual y no viola la distancia máxima,
        /// entonces guarda la ciudad candidata y su posición potencial. Una vez que
        /// ha recorrido todas las ciudades no visitadas, toma la mejor opción
        /// detectada y realiza a agregación al itinerario actual.
        /// 
        /// <param name="itinerary">
        /// <param name="limitDistance">
        /// <param name="scoredAttractions">
        /// @return </param>
        private OptimizedItinerary InsertBestPosition(OptimizedItinerary itinerary, double limitDistance, List<ScoredAttraction> scoredAttractions)
		{

			OptimizedItinerary itinerarioInsertado;

			double[] dblScoreDistPotencial = new double[] {0.0, limitDistance};
			double[] dblScoreDistTemp = new double[2];

			ScoredAttraction objEcpPotencial = new ScoredAttraction();
			bool blHayProspecto = false;
			int intPosicion = 1;

			itinerarioInsertado = itinerary;

			//1-Recorrer cada una de las ciudades que no han sido ya insertadas
			for (int j = 0; j < scoredAttractions.Count; j++)
			{
				if (!scoredAttractions[j].WasVisited)
				{

					//Debo revisar en cual posición vendría mejor sin tomar encuenta inicio y fin
					for (int i = 1; i < itinerary.Count - 1; i++)
					{
						dblScoreDistTemp = CalculatePotentialScoreDistanceByInsertion(itinerarioInsertado, scoredAttractions[j], i);
                        //Verificando que no incumpla la distancia límite y que haya mejoría en el score global y si se cumple se guarda como candidato.
						if (dblScoreDistTemp[1] <= limitDistance && dblScoreDistTemp[0] > dblScoreDistPotencial[0])
						{

							objEcpPotencial = (ScoredAttraction)scoredAttractions[j].Clone();
							intPosicion = i;
							dblScoreDistPotencial = dblScoreDistTemp;
							blHayProspecto = true;
						}
					} //Fin del for de la posición
				} //Fin del if de la entidad no visitada
			} //Fin del for de las entidades filtradas

            //Si se encuentra un prospecto a la inserción
			if (blHayProspecto)
			{
                //se marca como visita y se inserta en la posicion encontrada como mejorada
				objEcpPotencial.WasVisited = true;
				itinerarioInsertado.Insert(intPosicion, objEcpPotencial);

                //posicion en la matriz del objeto potencial
				int intId = objEcpPotencial.Position;
                //Lo de abajo lo cambiaría for 
                //alEntidades[intId].FueVisitada = true;
				for (int i = 0; i < scoredAttractions.Count; i++)
				{
					if (scoredAttractions[i].Position == intId)
					{
						scoredAttractions[i].WasVisited = true;
					}
				}

			}

			return itinerarioInsertado;
		}

        /// <summary>
        /// Quita 2 nodos consecutivos e intenta sustituirlos por otro único nodo que mejore el score y la distancia la mantenga menor a la límite
        /// 
        /// <param name="itinerary">
        /// <param name="limitDistance">
        /// <param name="scoredAttractions">
        /// @return </param>
        private OptimizedItinerary Remove2Insert1(OptimizedItinerary itinerary, double limitDistance, List<ScoredAttraction> scoredAttractions)
		{

			OptimizedItinerary itinerarioExtraido = new OptimizedItinerary();
			OptimizedItinerary itinerarioRetornado = new OptimizedItinerary();

			OptimizedItinerary objEliminadosTemp = new OptimizedItinerary();
			OptimizedItinerary objEliminadosFinal = new OptimizedItinerary();

			double[] dblScoreDistActual = new double[] {0.0, limitDistance};
			double[] dblScoreDistActualNew = new double[] {0.0, limitDistance};

			double[] dblScoreDistTemp = new double[2];
			ScoredAttraction objEcpPotencial = new ScoredAttraction();
			bool blHayProspecto = false;
			int intPosicion = 0;
           
			itinerarioExtraido.AddRange(itinerary);
			dblScoreDistActual = GetDistanceScore(itinerary);
            //itera del segundo nodo al penúltimo del itinerario
			for (int i = 1; i < itinerary.Count - 2; i++)
			{

				//Reinicio la RutaExtraida para seguir haciendo pruebas
				itinerarioExtraido.Clear();
				itinerarioExtraido.AddRange(itinerary);

				//Guardo los nodos eliminados para luego marcarlos como no visitados
				objEliminadosTemp.Add(itinerarioExtraido[i]);
				objEliminadosTemp.Add(itinerarioExtraido[i + 1]);

				itinerarioExtraido = ExtractSegment(itinerarioExtraido, i, i + 1);

				//Insertar el nodo en el "hueco" que dejarón los dos nodos removidos
				foreach (ScoredAttraction ecp in scoredAttractions)
				{
					if (ecp.WasVisited == false)
					{

						dblScoreDistTemp = CalculatePotentialScoreDistanceByInsertion(itinerarioExtraido, ecp, i);

						//Si se obtiene un mejor Score y no se viola la distancia máxima
						if (dblScoreDistTemp[0] > dblScoreDistActual[0] && dblScoreDistTemp[1] <= limitDistance)
						{
							//Si cumple la condición entonces hacer la inserción
							objEcpPotencial = (ScoredAttraction)ecp.Clone();
							intPosicion = i;
							objEliminadosFinal.Clear();
							objEliminadosFinal.AddRange(objEliminadosTemp);
							blHayProspecto = true;
						}
					}
				} //Fin del for de las ciudades no visitadas

				objEliminadosTemp.Clear();

			} //Fin del for del itinerario
			itinerarioExtraido.Clear();
			itinerarioExtraido.AddRange(itinerary);

			if (blHayProspecto)
			{

				//Hacer la verdadera extracción
				itinerarioExtraido = ExtractSegment(itinerarioExtraido, intPosicion, intPosicion + 1);

				//Marcar como no visitados
				foreach (ScoredAttraction ecp in objEliminadosFinal)
				{
                    //Acá se alteró por el generador de código la lógica
					int intId1 = ecp.Position;

                    scoredAttractions[intId1].WasVisited = false;
					//alEntidades.Where(c => c.Position.Equals(intId1)).First().FueVisitada = false;

				}

				//Marcar como visitada
                //Acá cambio lógica debido a la conversión de java a c#
				int intId = objEcpPotencial.Position;
                scoredAttractions[intId].WasVisited = true;
				//alEntidades.Where(c => c.Position.Equals(intId)).First().FueVisitada = true;

				itinerarioExtraido.Insert(intPosicion, objEcpPotencial);
				itinerarioRetornado.Clear();
				itinerarioRetornado.AddRange(itinerarioExtraido);
			}
			else
			{
				itinerarioRetornado.Clear();
				itinerarioRetornado.AddRange(itinerary);
			}

			return itinerarioRetornado;
		}

		/// <summary>
		/// Remueve un segmento de una ruta, se le pasa el inicio y el fin del
		/// segmento a remover. Los elementos en la posición de inicio y de fin
		/// también serán removidos. El segmento es continuo. No se hace una
		/// simulación, sino que elimina los nodos y listo.
		/// </summary>
		/// <param name="itinerary"> </param>
		/// <param name="startPos"> </param>
		/// <param name="finalPos">
		/// @return </param>
		private static OptimizedItinerary ExtractSegment(OptimizedItinerary itinerary, int startPos, int finalPos)
		{

			OptimizedItinerary objRutaRecortada = new OptimizedItinerary();

			objRutaRecortada.AddRange(itinerary);
			if (finalPos < objRutaRecortada.Count && startPos > 0 && startPos <= finalPos)
			{
				for (int i = finalPos; i >= startPos; i--)
				{
					objRutaRecortada.RemoveAt(i);
				}
			}

			return objRutaRecortada;
		}

		/// <summary>
		/// <param name="itinerary">
		/// @return </param>
		private static OptimizedItinerary RandomRemoval(OptimizedItinerary itinerary, List<ScoredAttraction> scoredAttractions)
		{
			int intAleatorio;
			ScoredAttraction objEcp;

			Random r = new Random();

            //Se genera una posición aleatoria entre el segundo nodo del itinerario y el penúltimo
            //intAleatorio = r.Next(1, itinerary.Count - 1);//inclusivo en lower bound y exclusivo en upper bound
            //Esto evita que elimine el origen del itinerario
            intAleatorio = r.Next(itinerary.Count - 2) + 1;

			////Esto evita que elimine el final del itinerario
			while (intAleatorio == itinerary.Count - 1)
			{
				intAleatorio = r.Next(itinerary.Count - 2) + 1;
			}
            
            //se obtiene referencia del objeto a borrar como tal
			objEcp = itinerary[intAleatorio];
            //finalmente borramos el item del itinerario en la posición aleatoria
            itinerary.RemoveAt(intAleatorio);
            //El objeto sigue siendo valido ya que no hemos perdido referencia al mismo.
			int intId = objEcp.Position;

            //Marco la entidad como no visitada
            scoredAttractions[intId].WasVisited = false;


			return itinerary;
		}

		private  double[] GetDistanceScore(OptimizedItinerary itinerary)
		{

			double subTotalScores1 = 0.0;
			double subTotalScores2 = 0.0;
			double subTotalScores3 = 0.0;
			double subTotalScores4 = 0.0;

			double TotalScores = 0.0;
			double subDistancia = 0.0;
			double[] dblDistanciaScore = new double[2];

			for (int j = 0; j < itinerary.Count - 1; j++)
			{
				subTotalScores1 += Math.Pow(itinerary[j].ArtScore, kExponent);
				subTotalScores2 += Math.Pow(itinerary[j].HistoryScore, kExponent);
				subTotalScores3 += Math.Pow(itinerary[j].ReligionScore, kExponent);
				subTotalScores4 += Math.Pow(itinerary[j].NatureScore, kExponent);
				subDistancia += Distances[itinerary[j].Position][itinerary[j + 1].Position];
			}

			subTotalScores1 = Math.Pow(subTotalScores1, (double) 1 / kExponent);
			subTotalScores2 = Math.Pow(subTotalScores2, (double) 1 / kExponent);
			subTotalScores3 = Math.Pow(subTotalScores3, (double) 1 / kExponent);
			subTotalScores4 = Math.Pow(subTotalScores4, (double) 1 / kExponent);

			TotalScores += subTotalScores1 * touristWeights[0];
			TotalScores += subTotalScores2 * touristWeights[1];
			TotalScores += subTotalScores3 * touristWeights[2];
			TotalScores += subTotalScores4 * touristWeights[3];

			dblDistanciaScore[0] = subDistancia;
			dblDistanciaScore[1] = TotalScores;

			return dblDistanciaScore;
		}

		/// <summary>
		/// <param name="itinerary">
		/// @return </param>
		private  double CalculateItineraryDistance(OptimizedItinerary itinerary)
		{

			double dblSubDistancia = 0.0;
			double dblDistancia = 0.0;
			int i;

			//Distancias desde la primera hasta la ultima
			for (i = 0; i < itinerary.Count - 1; i++)
			{

				//Usando matriz de distancias
				dblSubDistancia = Distances[itinerary[i].Position][itinerary[i + 1].Position];
				dblDistancia = dblDistancia + dblSubDistancia;
			}

			return dblDistancia;
		}

        //Se simula como que se quiere insertar en la posicion dada como argumento
        //ecp es la atraccion con la que luego se evalua su candidatura, posicion se manda desde el greedy el ultimo nodo
		private  double[] CalculatePotentialScoreDistanceByInsertion(OptimizedItinerary itinerary, ScoredAttraction scoredAttraction, int position)
		{
			//double subTotalScores = 0.0;

			double subTotalScores1 = 0.0;
			double subTotalScores2 = 0.0;
			double subTotalScores3 = 0.0;
			double subTotalScores4 = 0.0;

			double TotalScores = 0.0;
			double subDistancia = 0.0;
			double[] dblResultado = new double[2];

            //Aca termina justo antes de no haber navegado al final del itinerario greedy
			for (int j = 0; j < itinerary.Count - 1; j++)
			{
                //Va sumando el score de cada itinerario del greedy o de itinerario a evaluar elevado a una potencia K,
				subTotalScores1 += Math.Pow(itinerary[j].ArtScore, kExponent);
				subTotalScores2 += Math.Pow(itinerary[j].HistoryScore, kExponent);
				subTotalScores3 += Math.Pow(itinerary[j].ReligionScore, kExponent);
				subTotalScores4 += Math.Pow(itinerary[j].NatureScore, kExponent);
                //Utiliza Position como tal pero entonces este id entidad tendría que representar el orden dentro de la matriz quizas
                //va hacia la fija que pertenece la entidad o atraccion dentro de la matriz y luego se dirige a la columna de la siguiente atraccion a iterar para asi saber la distancia y sumarla
				subDistancia += Distances[itinerary[j].Position][itinerary[j + 1].Position];
			}
            //Aca veo aplica la sumatoria del score de la atraccion como parametro que se le pasa y que se le suma a lo que se calculo a partir del itinerario greedy o argumento
			subTotalScores1 += Math.Pow(scoredAttraction.ArtScore, kExponent);
			subTotalScores2 += Math.Pow(scoredAttraction.HistoryScore, kExponent);
			subTotalScores3 += Math.Pow(scoredAttraction.ReligionScore, kExponent);
			subTotalScores4 += Math.Pow(scoredAttraction.NatureScore, kExponent);

			subTotalScores1 = Math.Pow(subTotalScores1, (double) 1 / kExponent);
			subTotalScores2 = Math.Pow(subTotalScores2, (double) 1 / kExponent);
			subTotalScores3 = Math.Pow(subTotalScores3, (double) 1 / kExponent);
			subTotalScores4 = Math.Pow(subTotalScores4, (double) 1 / kExponent);

            //el score total se hace sumando los productos con los pesos
			TotalScores += subTotalScores1 * touristWeights[0];
			TotalScores += subTotalScores2 * touristWeights[1];
			TotalScores += subTotalScores3 * touristWeights[2];
			TotalScores += subTotalScores4 * touristWeights[3];
            //Se obtiene el score total con esa atraccion para evaluar su candidatura
			dblResultado[0] = TotalScores;

	//Calculando la nueva distancia potencial------------------------------ 
			//Agregando la distancia al nodo final.  tomando la ultima atraccion del itinerario como fila y como columna la posicion de la entidad origen
            //Distancia de ultima visita con origen
			subDistancia += Distances[itinerary[itinerary.Count - 1].Position][originAttractionPosition];

			//subDistancia = itinerario.getDistancia();
			//Como se desconectaran un par de nodos para permitir la entrada del nuevo nodo,
			//Tengo que restar también esa distancia
            //En el contexto del greedy posicion es siempre el ultimo indice del array entonces se hace con la fila de la penultima y su distancia con la ultima
            //Resta de distancia anterior a la de argumento con la dada como argumento
			subDistancia -= Distances[itinerary[position - 1].Position][itinerary[position].Position];

			//Calculando la distancias al potencial nodo  tomando en cuenta el penultimo dado que el ultimo se toma como que se desconecta del ultimo, y hacia el potencial nodo
            //suma distancia del penultimo al nodo potencial
            //suma de la anterior a la posicion dada como argumento conectada a la potencial
			subDistancia += Distances[itinerary[position - 1].Position][scoredAttraction.Position];
            //suma ahora del nodo potencial hacia el que esta de ultimo en el itinerario que se pasó
            //En esta logica yo entiendo que se hace con el penultimo ya que inicia y acaba en el mismo punto y de insertarse un nodo seria de penultimo para respetar lo mismo
            //conectando la potencial con la de la posicion dada como argumento
			subDistancia += Distances[scoredAttraction.Position][itinerary[position].Position];
			dblResultado[1] = subDistancia;

			return dblResultado;
		}

		/// <summary>
		/// Busca entre cada uno de los nodos del itinerario el nodo que al ser
		/// movido a otra posición reduzca en mayor grado la distancia del
		/// itinerario. Este ajuste solo se realiza una vez.
		/// 
		/// <param name="itinerary">
		/// @return </param>
		private  OptimizedItinerary MoveToBestPosition(OptimizedItinerary itinerary)
		{
			OptimizedItinerary itinerarioDesplazado = new OptimizedItinerary();

			double dblDistPotencial = CalculateItineraryDistance(itinerary);
			double dblDistTemp;
			ScoredAttraction objEcpPotencial = new ScoredAttraction();
			bool blHayProspecto = false;
			int intPosicionOriginal = 1;
			int intPosicionFinal = 1;

			itinerarioDesplazado = itinerary;

			//1-Recorrer cada una de las atracciones del itinerario sin incluir inicio y fin
			for (int i = 1; i < itinerary.Count - 1; i++)
			{

				//Debo revisar en cual posición vendría mejor
				for (int j = 1; j < itinerary.Count - 1; j++)
				{
                    //se compara solo si la posición nueva es distinta a la actual
					if (i != j)
					{
						dblDistTemp = CalculatePotentialDistanceByOneDisplacement(itinerarioDesplazado, i, j);
                        //Se verifica si hay una nueva distancia minimizada por dicho movimiento
						if (dblDistTemp <= dblDistPotencial)
						{
							objEcpPotencial = itinerary[i];
							intPosicionOriginal = i;
							intPosicionFinal = j;

							blHayProspecto = true;
							dblDistPotencial = dblDistTemp;
						}
					}
				} //Fin del for de la j

			}

			if (blHayProspecto)
			{
				//La elimino de su posición original en el itinerario
				itinerarioDesplazado.RemoveAt(intPosicionOriginal);

				//Aquí la inserto en la posición destino
				itinerarioDesplazado.Insert(intPosicionFinal, objEcpPotencial);
			}

			return itinerarioDesplazado;
		}

		/// <summary>
		/// El objetivo es ver como afecta a la distancia del itinerio el mover de
		/// lugar un nodo.
		/// 
		/// <param name="itinerary"> </param>
		/// <param name="objECP"> </param>
		/// <param name="finalPosition">
		/// @return </param>
		private  double CalculatePotentialDistanceByOneDisplacement(OptimizedItinerary itinerary, int originalPosition, int finalPosition)
		{

			double dblResultado;
			double subDistancia = 0.0;

			subDistancia = CalculateItineraryDistance(itinerary);
            //Para evaluar desplazamiento del nodo hacia adelante en el itinerario, es decir si se atrasase la visita hasta que este en la posicion final
			if (originalPosition < finalPosition)
			{
                //Se resta la distancia de la conexión del nodo anterior al original con este
				subDistancia -= Distances[itinerary[originalPosition - 1].Position][itinerary[originalPosition].Position];
                //Se resta la distancia del nodo con el siguiente a este
				subDistancia -= Distances[itinerary[originalPosition].Position][itinerary[originalPosition + 1].Position];
                //Se resta la distancia que va desde el nodo que esta en la posicion final hacia el siguiente
				subDistancia -= Distances[itinerary[finalPosition].Position][itinerary[finalPosition + 1].Position];

                //Se suma la distancia como origen el nodo anterior al original conectado simulado al siguiente de la original
				subDistancia += Distances[itinerary[originalPosition - 1].Position][itinerary[originalPosition + 1].Position];
                //Se suma la distancia que va desde el nodo de la posicion objetivo hacia el dado como original de forma que simulando que el nodo original fuese
                //luego del de la posición objetivo
				subDistancia += Distances[itinerary[finalPosition].Position][itinerary[originalPosition].Position];
                //Se suma la distancia del nodo original ahora hacia el que estaba de siguiente a la posicion objetivo
				subDistancia += Distances[itinerary[originalPosition].Position][itinerary[finalPosition + 1].Position];
			}

			if (originalPosition > finalPosition)
			{
				subDistancia += Distances[itinerary[finalPosition - 1].Position][itinerary[originalPosition].Position];
				subDistancia += Distances[itinerary[originalPosition].Position][itinerary[finalPosition].Position];
				subDistancia += Distances[itinerary[originalPosition - 1].Position][itinerary[originalPosition + 1].Position];

				subDistancia -= Distances[itinerary[originalPosition - 1].Position][itinerary[originalPosition].Position];
				subDistancia -= Distances[itinerary[originalPosition].Position][itinerary[originalPosition + 1].Position];
				subDistancia -= Distances[itinerary[finalPosition - 1].Position][itinerary[finalPosition].Position];
			}

			dblResultado = subDistancia;
			return dblResultado;
		}
	}

}