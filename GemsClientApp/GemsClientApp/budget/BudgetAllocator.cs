using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using evmsService.entities;

namespace Gems.UIWPF
{
    //Kok Wei & Beng Hee
    class State : IComparable<State>
    {
        private decimal priceTotal;

        public decimal TotalPrice
        {
            get { return priceTotal; }
            set { priceTotal = value; }
        }
        private int satisfactionTotal;

        public int TotalSatisfactionValue
        {
            get { return satisfactionTotal; }
            set { satisfactionTotal = value; }
        }
        private Items item;

        public Items Item
        {
            get { return item; }
            set { item = value; }
        }
        private State prevState;

        public State PrevState
        {
            get { return prevState; }
            set { prevState = value; }
        }

        public int CompareTo(State s)
        {
            return priceTotal.CompareTo(s.priceTotal);
        }
    }

    public class BudgetAllocator
    {
        private decimal minBudget;

        public decimal MinBudget
        {
            get { return minBudget; }
            set { minBudget = value; }
        }
        private decimal maxBudget;

        public decimal MaxBudget
        {
            get { return maxBudget; }
            set { maxBudget = value; }
        }
        private List<List<Items>> importantItemsByType;

        public List<List<Items>> ImportantItemsByType
        {
            get { return importantItemsByType; }
            set { importantItemsByType = value; }
        }
        private List<List<Items>> unimportantItemsByType;

        public List<List<Items>> UnimportantItemsByType
        {
            get { return unimportantItemsByType; }
            set { unimportantItemsByType = value; }
        }

        private List<State> DPtreeLeaves;

        public BudgetAllocator(List<Items> items, List<ItemTypes> itemTypes, decimal maxBudget)
        {
            importantItemsByType = new List<List<Items>>();
            unimportantItemsByType = new List<List<Items>>();

            Dictionary<string, List<Items>> itemsByType = new Dictionary<string, List<Items>>();
            foreach (Items item in items)
            {
                if (!itemsByType.ContainsKey(item.typeString))
                    itemsByType[item.typeString] = new List<Items>();
                itemsByType[item.typeString].Add(item);
            }
            foreach (ItemTypes itemType in itemTypes)
            {
                if (itemsByType.ContainsKey(itemType.typeString))
                    if (itemType.IsImportantType)
                    {
                        //Should be order by descending not ascending
                        //Test case for Impt Item Item1: $5, Item 2: $3
                        //max budget $3, .OrderBy threw exception insufficient budget
                        //for required item, .OrderByDescending did not throw
                        importantItemsByType.Add(itemsByType[itemType.typeString]
                           .OrderBy(i => i.EstimatedPrice).ToList());
                    }
                    else
                    {
                        //Should be order by descending not ascending
                        unimportantItemsByType.Add(itemsByType[itemType.typeString]
                           .OrderBy(i => i.EstimatedPrice).ToList());
                    }
            }

            minBudget = 0;

            foreach (List<Items> itemsList in importantItemsByType)
            {
                minBudget += itemsList[0].EstimatedPrice;
            }

            if (minBudget > maxBudget)
                throw new ArgumentOutOfRangeException
                    ("The maximum amount of budget was too small to obtain all required items");

            this.maxBudget = maxBudget;
            InitializeDPtree();
        }

        private void addItemToStates(Items item, IEnumerable<State> inStates, ICollection<State> outStates)
        {
            foreach (State state in inStates)
            {
                if (state.TotalPrice + item.EstimatedPrice > maxBudget)
                    return; //if it exceeds, dont bother 
                outStates.Add(new State()
                {
                    TotalPrice = state.TotalPrice + item.EstimatedPrice,
                    TotalSatisfactionValue = state.TotalSatisfactionValue + item.Satisfaction,
                    PrevState = state,
                    Item = item
                });
            }
        }

        public void InitializeDPtree()
        {
            List<State> prevStates = new List<State>();
            prevStates.Add(new State());
            foreach (List<Items> itemsList in importantItemsByType)
            {
                List<State> currStates = new List<State>();
                foreach (Items item in itemsList)
                {
                    addItemToStates(item, prevStates, currStates);
                }
                currStates.Sort();
                State prevState = new State();
                List<State> prunedStates = new List<State>();
                foreach (State currState in currStates)
                {
                    if (currState.TotalSatisfactionValue >= prevState.TotalSatisfactionValue)
                    {
                        //shld be >= not > 
                        prunedStates.Add(currState);
                        prevState = currState;
                    }
                }
                prevStates = prunedStates;
            }
            foreach (List<Items> itemsList in unimportantItemsByType)
            {
                List<State> currStates = new List<State>();
                foreach (Items item in itemsList)
                    addItemToStates(item, prevStates, currStates);
                currStates.Sort();
                prevStates.Add(new State() { TotalPrice = decimal.MaxValue });
                currStates.Add(new State() { TotalPrice = decimal.MaxValue });
                List<State> prunedStates = new List<State>();
                State prevState = new State() { TotalSatisfactionValue = -1 };
                for (int i = 0, j = 0; i < prevStates.Count || j < currStates.Count; )
                {
                    State currState = prevStates[i].TotalPrice <
                        currStates[j].TotalPrice ? prevStates[i++] : currStates[j++];
                    if (currState.TotalPrice == decimal.MaxValue)
                    { //If price is INF, break loop
                        break;
                    }
                    if (currState.TotalSatisfactionValue >= prevState.TotalSatisfactionValue)
                    {
                        //shld be >= not > 
                        prunedStates.Add(currState);
                        prevState = currState;
                    }
                }
                prevStates = prunedStates;
            }
            DPtreeLeaves = prevStates;
        }

        public List<List<Items>> optimalItems(decimal budget)
        {
            decimal maxPrice;
            int maxSatisfaction;

            if (budget < minBudget)
                throw new ArgumentOutOfRangeException("The amount of budget entered is insufficient");
            else if (budget > maxBudget)
                throw new ArgumentOutOfRangeException("Your entered budget is more than your maximum defined budget!");


            List<List<Items>> result = new List<List<Items>>();
            int i = DPtreeLeaves.Count - 1;
            for (; i >= 0; i--)
            {
                if (DPtreeLeaves[i].TotalPrice <= budget)
                    break; //from the last state, if totalprice is less than or equal to budget
                //break loop
            }
            //set the max price and satisfaction to the last state that can be used
            maxPrice = DPtreeLeaves[i].TotalPrice;
            maxSatisfaction = DPtreeLeaves[i].TotalSatisfactionValue;

            do
            {
                //if the total satisfaction = max satisfaction it is a valid result
                State stateOptimal = DPtreeLeaves[i];
                List<Items> itemsOptimal = new List<Items>();
                while (stateOptimal.PrevState != null)
                {
                    itemsOptimal.Add(stateOptimal.Item);
                    stateOptimal = stateOptimal.PrevState;
                }
                result.Add(itemsOptimal);

                i--;
            } while (i < DPtreeLeaves.Count && 
                (DPtreeLeaves[i].TotalSatisfactionValue == maxSatisfaction) &&
                DPtreeLeaves[i].TotalPrice <= maxPrice);

            //More items the better...
            return result.OrderByDescending(x => x.Count).ToList();
        }
    }
}
