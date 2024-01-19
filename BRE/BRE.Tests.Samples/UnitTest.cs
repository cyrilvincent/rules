using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BRE.Entities.Sample;
using BRE.RulesEngine;
using BRE.Rules.Sample;
using System.Collections.Generic;
using BRE.Rules;
using BRE.Workflow.Samples;
using BRE.Workflow;
using BRE.Entities;

namespace BRE.Test.Sample
{
    [TestClass]
    public class UnitTest
    {
        [TestInitialize]
        public void Init()
        {
            Console.WriteLine("Start UnitTest");
        }

        [TestCleanup]
        public void Cleanup()
        {
            Console.WriteLine("Stop UnitTest");
        }

        [TestMethod]
        public void TestOneEntity()
        {
            MyEntity e = SampleData.Data[0];
            RulesEngineProcess<MyEntity, EntityOutput> engine = new RulesEngineProcess<MyEntity, EntityOutput>();
            engine.RuleSet = new SampleRuleSet();
            engine.Variables = new SampleVariables();
            EntityOutput output = new EntityOutput();
            engine.Compile(e, output);
            string s = engine.Logs;
            Assert.AreEqual(RulesEngineStatus.Success, engine.Status);
        }

        [TestMethod]
        public void TestChainedGame()
        {
            GameEntity game = new GameEntity { Max = 100, Try = 50, Value = 74 };
            ChainedRulesEngineProcess<GameEntity, GameOutput> engine = new ChainedRulesEngineProcess<GameEntity, GameOutput>();
            engine.RuleSet = new SampleGameRuleSet();
            engine.Input = game;
            GameOutput output = engine.Compile();
            string s = engine.Logs;
            Assert.AreEqual(game.Value, output.Value);
            Assert.AreEqual(RulesEngineStatus.Success, engine.Status);
        }

        [TestMethod]
        public void FailedGame()
        {
            GameEntity game = new GameEntity { Max = 0, Min = 1 };
            ChainedRulesEngineProcess<GameEntity, GameOutput> engine = new ChainedRulesEngineProcess<GameEntity, GameOutput>();
            engine.RuleSet = new SampleGameRuleSet();
            engine.Input = game;
            GameOutput output = engine.Compile();
            string s = engine.Logs;
            Assert.AreEqual(RulesEngineStatus.Failed, engine.Status);
        }

        [TestMethod]
        public void TestInfiniteChainedGame()
        {
            GameEntity game = new GameEntity { Max = 100, Try = 50, Value = 74 };
            ChainedRulesEngineProcess<GameEntity, GameOutput> engine = new ChainedRulesEngineProcess<GameEntity, GameOutput>();
            engine.RuleSet = new SampleInfiniteRuleSet();
            engine.Input = game;
            GameOutput output = engine.Compile();
            string s = engine.Logs;
            Assert.AreEqual(RulesEngineStatus.Failed, engine.Status);
            Assert.AreEqual(engine.NbIterationMax, engine.Iteration);
        }

        [TestMethod]
        public void TestEmptyChainedGame()
        {
            GameEntity game = new GameEntity();
            ChainedRulesEngineProcess<GameEntity, GameOutput> engine = new ChainedRulesEngineProcess<GameEntity, GameOutput>();
            engine.RuleSet = new RuleSet<GameEntity, GameOutput>();
            engine.Input = game;
            GameOutput output = engine.Compile();
            string s = engine.Logs;
            Assert.AreEqual(RulesEngineStatus.Success, engine.Status);
        }

        [TestMethod]
        public void TestWorkflow()
        {
            WorkflowSample wf = new WorkflowSample();
            Variables v = new Variables();
            v.Add(new Variable { Name = "Text", Value = "Congratulation {0} the value is {1}." });
            wf.Variables = v;
            wf.Input = new GameEntity { Max = 100, Value = 66, Try = 50 };
            bool b = wf.Start();
            Assert.IsTrue(b);
            Assert.AreEqual("Congratulation Toto the value is 66.", wf.Output.Value);
        }

        [TestMethod]
        public void TestLargeRuleSet()
        {
            LargeRuleSetActivitySample a = new LargeRuleSetActivitySample();
            a.Input = new MyEntity();
            bool b = a.Compile();
            Assert.IsFalse(b);
            MyEntity e = a.Input;
            Assert.IsNotNull(e);
            Assert.AreEqual(SampleLargeRuleSet.Nb, e.MyInt);
        }

        [TestMethod]
        public void TestSimpleActivity()
        {
            SimpleActivity<MyEntity> a = new SimpleActivity<MyEntity>();
            a.Input = new MyEntity();
            a.RuleSet = new SampleLargeRuleSet();
            bool b = a.Compile();
            Assert.IsFalse(b);
            MyEntity e = a.Input;
            Assert.IsNotNull(e);
            Assert.AreEqual(SampleLargeRuleSet.Nb, e.MyInt);
        }

        [TestMethod]
        public void TestElseAction()
        {
            GameEntity game = new GameEntity { Max = 100, Try = 50, Value = 74 };
            ChainedRulesEngineProcess<GameEntity, GameOutput> engine = new ChainedRulesEngineProcess<GameEntity, GameOutput>();
            engine.RuleSet = new SampleGameElseRuleSet();
            engine.Input = game;
            GameOutput output = engine.Compile();
            string s = engine.Logs;
            Assert.AreEqual(game.Value, output.Value);
            Assert.AreEqual(RulesEngineStatus.Success, engine.Status);
        }

       
    }
}
