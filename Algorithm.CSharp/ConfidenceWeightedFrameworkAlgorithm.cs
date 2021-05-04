/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Collections.Generic;
using QuantConnect.Algorithm.Framework.Alphas;
using QuantConnect.Algorithm.Framework.Execution;
using QuantConnect.Algorithm.Framework.Portfolio;
using QuantConnect.Algorithm.Framework.Selection;
using QuantConnect.Interfaces;

namespace QuantConnect.Algorithm.CSharp
{
    /// <summary>
    /// Test algorithm using <see cref="ConfidenceWeightedPortfolioConstructionModel"/> and <see cref="ConstantAlphaModel"/>
    /// generating a constant <see cref="Insight"/> with a 0.25 confidence
    /// </summary>
    public class ConfidenceWeightedFrameworkAlgorithm : QCAlgorithm, IRegressionAlgorithmDefinition
    {
        /// <summary>
        /// Initialise the data and resolution required, as well as the cash and start-end dates for your algorithm. All algorithms must initialized.
        /// </summary>
        public override void Initialize()
        {
            // Set requested data resolution
            UniverseSettings.Resolution = Resolution.Minute;

            SetStartDate(2013, 10, 07);  //Set Start Date
            SetEndDate(2013, 10, 11);    //Set End Date
            SetCash(100000);             //Set Strategy Cash

            // set algorithm framework models
            SetUniverseSelection(new ManualUniverseSelectionModel(QuantConnect.Symbol.Create("SPY", SecurityType.Equity, Market.USA)));
            SetAlpha(new ConstantAlphaModel(InsightType.Price, InsightDirection.Up, TimeSpan.FromMinutes(20), 0.025, 0.25));
            SetPortfolioConstruction(new ConfidenceWeightedPortfolioConstructionModel());
            SetExecution(new ImmediateExecutionModel());
        }

        public override void OnEndOfAlgorithm()
        {
            if (// holdings value should be 0.25 - to avoid price fluctuation issue we compare with 0.28 and 0.23
                Portfolio.TotalHoldingsValue > Portfolio.TotalPortfolioValue * 0.28m
                ||
                Portfolio.TotalHoldingsValue < Portfolio.TotalPortfolioValue * 0.23m)
            {
                throw new Exception($"Unexpected Total Holdings Value: {Portfolio.TotalHoldingsValue}");
            }
        }

        /// <summary>
        /// This is used by the regression test system to indicate if the open source Lean repository has the required data to run this algorithm.
        /// </summary>
        public bool CanRunLocally { get; } = true;

        /// <summary>
        /// This is used by the regression test system to indicate which languages this algorithm is written in.
        /// </summary>
        public Language[] Languages { get; } = { Language.CSharp, Language.Python };

        /// <summary>
        /// This is used by the regression test system to indicate what the expected statistics are from running the algorithm
        /// </summary>
        public Dictionary<string, string> ExpectedStatistics => new Dictionary<string, string>
        {
            {"Total Trades", "6"},
            {"Average Win", "0%"},
            {"Average Loss", "0.00%"},
            {"Compounding Annual Return", "38.832%"},
            {"Drawdown", "0.600%"},
            {"Expectancy", "-1"},
            {"Net Profit", "0.420%"},
            {"Sharpe Ratio", "5.579"},
            {"Probabilistic Sharpe Ratio", "67.318%"},
            {"Loss Rate", "100%"},
            {"Win Rate", "0%"},
            {"Profit-Loss Ratio", "0"},
            {"Alpha", "-0.184"},
            {"Beta", "0.248"},
            {"Annual Standard Deviation", "0.055"},
            {"Annual Variance", "0.003"},
            {"Information Ratio", "-10.012"},
            {"Tracking Error", "0.167"},
            {"Treynor Ratio", "1.241"},
            {"Total Fees", "$6.00"},
            {"Estimated Strategy Capacity", "$33000000.00"},
            {"Fitness Score", "0.063"},
            {"Kelly Criterion Estimate", "38.796"},
            {"Kelly Criterion Probability Value", "0.228"},
            {"Sortino Ratio", "79228162514264337593543950335"},
            {"Return Over Maximum Drawdown", "70.89"},
            {"Portfolio Turnover", "0.063"},
            {"Total Insights Generated", "100"},
            {"Total Insights Closed", "99"},
            {"Total Insights Analysis Completed", "99"},
            {"Long Insight Count", "100"},
            {"Short Insight Count", "0"},
            {"Long/Short Ratio", "100%"},
            {"Estimated Monthly Alpha Value", "$117277.2200"},
            {"Total Accumulated Estimated Alpha Value", "$18894.6632"},
            {"Mean Population Estimated Insight Value", "$190.8552"},
            {"Mean Population Direction", "53.5354%"},
            {"Mean Population Magnitude", "53.5354%"},
            {"Rolling Averaged Population Direction", "58.2788%"},
            {"Rolling Averaged Population Magnitude", "58.2788%"},
            {"OrderListHash", "21e4704a124ba562d042e1e9962f4316"}
        };
    }
}
