//class FixtureRunnerSpec {

var FixtureRunnerSpec = function() {}

FixtureRunnerSpec.prototype = {  
  name: "FixtureRunner spec",    
  areResultsEqual: 
    function (expectedResult, actualResult) {
      if (expectedResult.test != actualResult.test) {
        throw "Test name different. Expected '" 
              + expectedResult.test + "', was '" 
              + actualResult.test + "'.";
      }
      if (expectedResult.isSuccessful != actualResult.isSuccessful) {
        throw "Test result different. Expected '" 
              + expectedResult.isSuccessful + "', was '" 
              + actualResult.isSuccessful + "'.";
      }
      return true;
    },
  resultComparer:
    function(first, second) {
      return (first.test == second.test && first.isSuccessful == second.isSuccessful);
    },
  should_pass_with_valid_assertion:
    function() {
      var mockFixture = function() {};
      mockFixture.prototype.testThatShouldPass = function() { Assert.areEqual(1+1, 2); }      
      var runner = new FixtureRunner().setFixture(mockFixture).run();                  
      var results = runner.getResults();
      var expected = new TestResult("testThatShouldPass", true);
      Assert.areEqual(1, results.length, "Expected one result");
      this.areResultsEqual(expected, results[0]);
    },
  should_not_run_functions_with_arguments_as_tests:
    function() {
      var mockFixture = function() {};
      mockFixture.prototype.nonTest = function(someArg) { Assert.areEqual(1, 2); }
      var runner = new FixtureRunner().setFixture(mockFixture).run();
      var results = runner.getResults();
      Assert.areEqual(results.length, 0);      
    },
  should_fail_with_invalid_assertion:
    function() {
      var mockFixture = function() {};
      mockFixture.prototype.testThatShouldFail = function() { Assert.areEqual(1, 2); }
      var runner = new FixtureRunner().setFixture(mockFixture).run();
      var results = runner.getResults();      
      Assert.areEqual(1, results.length, "Expected one result");
      Assert.areEqual("testThatShouldFail", results[0].test, "Test name incorrect");
      Assert.areEqual(false, results[0].isSuccessful, "Test should have failed");
    },
  should_have_no_side_effects_per_test:
    function() {
      var mockFixture = function() {};
      mockFixture.prototype.hasTestRun = false;
      mockFixture.prototype.testThatSetsState = 
        function() { 
          Assert.areEqual(false, this.hasTestRun); 
          this.hasTestRun = true; 
          Assert.areEqual(true, this.hasTestRun);
        }
      mockFixture.prototype.anotherStateAlteringTest = 
        function() {
          Assert.areEqual(false, this.hasTestRun); 
          this.hasTestRun = true; 
          Assert.areEqual(true, this.hasTestRun);
        }
      var runner = new FixtureRunner().setFixture(mockFixture).run();
      var results = runner.getResults();
      var expected = new Array();
      expected[0] = new TestResult("testThatSetsState", true);
      expected[1] = new TestResult("anotherStateAlteringTest", true);
      Assert.areEqual(2, results.length, "Expected two results");      
      for (var i=0; i<expected.length; i++) {
        Assert.that(expected[i], Is.In(results).UsingComparer(this.resultComparer));        
      }      
    },
  should_be_able_to_run_tests_defined_on_object:
    function() {
      var mockFixture = new Object();
      mockFixture.testTrueAssertion = function() { Assert.areEqual(1, 1); }
      mockFixture.testFalseAssertion = function() { Assert.areEqual(1, 2); }
      var runner = new FixtureRunner().setFixture(mockFixture).run();
      var results = runner.getResults();
      var expected = new Array();
      expected[0] = new TestResult("testTrueAssertion", true);
      expected[1] = new TestResult("testFalseAssertion", false);
      Assert.areEqual(2, results.length, "Expected two results");
      for (var i=0; i<expected.length; i++) {
        Assert.that(expected[i], Is.In(results).UsingComparer(this.resultComparer));        
      }      
    },
  should_be_able_to_run_tests_defined_on_class:
    function() {
      var mockFixture = function() {}
      mockFixture.prototype = {
        testTrueAssertion: function() { Assert.areEqual(1, 1); },
        testFalseAssertion: function() { Assert.areEqual(1, 2); }
      }
      var runner = new FixtureRunner().setFixture(mockFixture).run();
      var results = runner.getResults();
      var expected = new Array();
      expected[0] = new TestResult("testTrueAssertion", true);
      expected[1] = new TestResult("testFalseAssertion", false);
      Assert.areEqual(2, results.length, "Expected two results");
      for (var i=0; i<expected.length; i++) {
        Assert.that(expected[i], Is.In(results).UsingComparer(this.resultComparer));        
      }      
    },
  should_be_able_to_reuse_fixture_runner:
    function() {
      var fixture = new Object();
      fixture.testRunZero = function() { Assert.areEqual(1,1); }
      var runner = new FixtureRunner().setFixture(fixture).run();
      var results = runner.getResults();
      Assert.areEqual(1, results.length);
      this.areResultsEqual(new TestResult("testRunZero", true), results[0]);
      
      var newFixture = new Object();
      newFixture.testRunOne = function() { Assert.areEqual(1,2); }
      runner.setFixture(newFixture).run();
      results = runner.getResults();
      Assert.areEqual(1, results.length);
      this.areResultsEqual(new TestResult("testRunOne", false), results[0]);      
    },
  should_be_able_to_get_number_of_passes_and_fails:
    function() {      
      var runner = new FixtureRunner().setFixture({
                      name: "Mock fixture",
                      pass1: function() { Assert.areEqual(1,1); },
                      pass2: function() { Assert.areEqual(2,2); },
                      fail: function() { Assert.areEqual(1,2); }
                    }).run();      
      Assert.areEqual(2, runner.passCount);
      Assert.areEqual(1, runner.failCount);
      Assert.areEqual(runner.getResults().length, runner.passCount + runner.failCount);
    }
}


//}