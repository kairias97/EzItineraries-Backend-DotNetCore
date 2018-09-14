﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesApi.Models
{
    public interface IItineraryGenerator
    {
        ItineraryProposal GenerateProposal(ItineraryRequest request);
    }
}
