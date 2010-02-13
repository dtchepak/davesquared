var TestRunnerSpec = function(){}
TestRunnerSpec.prototype = {
  name: "TestRunner spec",
  should_have_zero_fixtures_when_created: function() {
    var runner = new TestRunner();
    Assert.areEqual(0, runner.fixtures.length);
  },
  should_be_able_to_add_fixtures: function() {
    var runner = new TestRunner();
    runner.addFixture( { name: "Test1" });
    runner.addFixture( { name: "Test2" });
    Assert.areEqual(2, runner.fixtures.length);
  },
  should_be_able_to_run_fixtures: function() {
    var runner = new TestRunner();
    var runCount = 0;
    runner.addFixture({name: "Test1", doCountIncrement: function(){runCount++;}});
    runner.addFixture({name: "Test2", doCountIncrement2: function(){runCount++;}});
    runner.run();
    Assert.areEqual(2, runCount);
  },
  should_have_correct_number_of_results_after_running_fixtures: function() {
    var runner = new TestRunner();
    runner.addFixture({name: "1"}).addFixture({name: "2"});
    Assert.areEqual(0, runner.results.length);
    runner.run();
    Assert.areEqual(2, runner.results.length);
    
  }  
}