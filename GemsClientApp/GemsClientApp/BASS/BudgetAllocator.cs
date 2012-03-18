using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using evmsService.entities;

namespace Gems.UIWPF.BASS
{
    class State : IComparable<State>
    {
        //Change to decimals
        //Item and ItemTypes exposed Properties can be found on the server
        public decimal priceTotal;
        public int satisfactionTotal;
        public Items item;
        public State prevState;

        public int CompareTo(State s)
        {
            return priceTotal.CompareTo(s.priceTotal);
        }
    }

    public class BudgetAllocator
    {
        public decimal budget;
        public List<List<Items>> itemsByType;
        public List<Items> itemsOptimal;
        public decimal priceOptimal;
        public int satisfactionOptimal;

        private void addItemToStates(Items item, IEnumerable<State> inStates, ICollection<State> outStates)
        {
            foreach (State state in inStates)
            {
                if (state.priceTotal + item.EstimatedPrice > budget)
                    return;
                outStates.Add(new State()
                {
                    priceTotal = state.priceTotal + item.EstimatedPrice,
                    satisfactionTotal = state.satisfactionTotal + item.Satisfaction,
                    prevState = state,
                    item = item
                });
            }
        }

        public void Allocate()
        {
            List<State> prevStates = new List<State>();
            prevStates.Add(new State());
            for (int i = 1; i < itemsByType.Count; i++) //required items
            {
                List<State> currStates = new List<State>();
                foreach (Items item in itemsByType[i])
                    addItemToStates(item, prevStates, currStates);

                if (currStates.Count == 0)
                {
                    Console.WriteLine("Budget too small to obtain all required items");
                    //change to messagebox or throw new exception..
                    return;
                }
                currStates.Sort();
                State prevState = new State();
                List<State> prunedStates = new List<State>();
                foreach (State currState in currStates)
                {
                    if (currState.satisfactionTotal > prevState.satisfactionTotal)
                    {
                        prunedStates.Add(currState);
                        prevState = currState;
                    }
                }
                prevStates = prunedStates;
                Console.WriteLine(prunedStates.Count);
            }
            foreach (Items item in itemsByType[0]) //optional items
            {
                List<State> currStates = new List<State>();
                addItemToStates(item, prevStates, currStates);

                //Changed to int.MaxValue instead of double.infinity
                prevStates.Add(new State() { priceTotal = int.MaxValue }); 
                currStates.Add(new State() { priceTotal = int.MaxValue });
                List<State> prunedStates = new List<State>();
                State prevState = new State() { satisfactionTotal = -1 };
                for (int i = 0, j = 0; i < prevStates.Count || j < currStates.Count; )
                {
                    State currState = prevStates[i].priceTotal < 
                        currStates[j].priceTotal ? prevStates[i++] : currStates[j++];

                    double p = (Double)currState.priceTotal;
                    
                    if (p == int.MaxValue) //changed isInfinity to compare to int.MaxValue
                        break;

                    if (currState.satisfactionTotal > prevState.satisfactionTotal)
                    {
                        prunedStates.Add(currState);
                        prevState = currState;
                    }
                }
                prevStates = prunedStates;
                Console.WriteLine(prevStates.Count);
            }
            State stateOptimal = prevStates[prevStates.Count - 1];
            priceOptimal = stateOptimal.priceTotal;
            satisfactionOptimal = stateOptimal.satisfactionTotal;
            itemsOptimal = new List<Items>();
            while (stateOptimal.prevState != null)
            {
                itemsOptimal.Add(stateOptimal.item);
                stateOptimal = stateOptimal.prevState;
            }
        }
    }


}

