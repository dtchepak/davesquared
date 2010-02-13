//class TestRunner{
var TestRunner = function() {
  this.fixtures = [];
  this.results = [];
}
TestRunner.prototype = {  
  passCount: 0,
  failCount: 0,
  runTimeInMilliseconds: 0,
  addFixture: function(fixtureReference) {
    this.fixtures[this.fixtures.length] = fixtureReference;
    return this;
  },
  run: function() {       
    this.resetCounters();
    this.results = [];
    var start = new Date();
    for (var i=0; i<this.fixtures.length; i++) {      
      var fixtureRunner = new FixtureRunner();
      fixtureRunner.setFixture(this.fixtures[i]).run();      
      var results = fixtureRunner.getResults();
      results.name = fixtureRunner.getFixtureName();
      this.results[this.results.length] = results;
      this.addToCounters(fixtureRunner.passCount, fixtureRunner.failCount);      
    }        
    this.runTimeInMilliseconds = new Date().getTime() - start.getTime();
  },
  addToCounters: function(passCount, failCount) {
    this.passCount += passCount;
    this.failCount += failCount;    
  },
  resetCounters: function() {
    this.passCount = 0;
    this.failCount = 0;    
  },
  getResults: function() {
    return this.results;
  }
}
//}

//class FixtureRunner{
var FixtureRunner = function() {}

FixtureRunner.prototype = {  
  fixture: null,  
  results: null,
  passCount: 0,
  failCount: 0,
  setFixture:
    function(fixtureReference) {
      if (this.isClass(fixtureReference)) {
        this.fixture = fixtureReference;
      } else {
        this.fixture = this.makeClassFromObject(fixtureReference);
      }
      return this;
    },
  isClass:
    function(reference) {
      return (typeof reference == "function");
    },
  makeClassFromObject:
    function(objectReference) {
      var classReference = function(){}
      classReference.prototype = objectReference;
      return classReference;
    },
  getFixtureInstance:
    function() {
      return new this.fixture();
    },
  getFixtureName:
    function() {
      return this.getFixtureInstance().name || "(unnamed fixture)";
    },
  run:
    function() {
      var instance = this.getFixtureInstance();      
      this.results = new Array();
      for (var member in instance) {
        var caughtException = null;
        var objectUnderTest = this.getFixtureInstance();        
        if (!this.isFunctionWithZeroArguments(objectUnderTest[member])) continue;        
        try {
          objectUnderTest[member]();
        } catch (ex) {
          caughtException = ex;
        }         
        
        var result = new TestResult(
                                    member.toString(), 
                                    caughtException == null,
                                    this.formatException(caughtException));
        this.results[this.results.length] = result;
        if (result.isSuccessful) {
          this.passCount++;
        } else {
          this.failCount++;
        }
      }
      return this;
    },
  formatException:
    function(exception) {      
      if (!exception) return null;      
      if (exception instanceof AssertionError) {
        return exception.name + ": " + exception.message;
      } else if (exception.name) {
        return exception.name + ": " +exception.message + " \n(" + exception.fileName + ":" + exception.lineNumber + ")";
      } else {
        return exception;
      }
    },
  isFunctionWithZeroArguments:  
    function(member) {
      return typeof(member) == "function" && member.length == 0;
    },        
  getResults:
    function() {
      return this.results;
    }
}
//}

//class TestResult {
var TestResult = function(test, isSuccessful, message) {
  this.test = test;
  this.isSuccessful = isSuccessful;
  this.message = message;
} 

TestResult.prototype = {
  test: null,
  isSuccessful: true,
  message: null      
}
//}

//class ResultsWriter{
var ResultsWriter = function(outputStream) {
  this.output = (outputStream) ? outputStream : document;
}
ResultsWriter.prototype = {
  writeSummary: function(fixtureCount, passCount, failCount, runTimeInMilliseconds) {
    this.writeHeading("Summary");
    var colour = (failCount>0) ? "red" : "green";
    this.output.write("<span style='color: " + colour + ";'>" +
                      passCount + " pass, " + failCount + " fail in " + fixtureCount + " fixtures." +
                      " Run took " + runTimeInMilliseconds + "ms." +
                      "</span><br />"
    );
  },  
  writeHeading:
    function(heading) {
      this.output.write("<b>" + heading + "</b><br/>");
    },
  writeResults:
    function(runner) {    
      var results = runner.getResults();
      this.writeSummary(results.length, runner.passCount, runner.failCount, runner.runTimeInMilliseconds);
      for (var fixtureIndex=0; fixtureIndex<results.length; fixtureIndex++) {
        var resultsForFixture = results[fixtureIndex];
        this.writeHeading(resultsForFixture.name);        
        for (var i=0; i<resultsForFixture.length; i++) {          
          this.writeResult(resultsForFixture[i]);
        }
      }
    },  
  writeResult:
    function(testResult) {
      var colour = testResult.isSuccessful ? "green" : "red";
      this.output.write("<span style='color: " + colour + ";'>" + testResult.test + "</span>");
      if (testResult.message != null) {
        this.output.write("<br/>&nbsp; &nbsp; &nbsp; &nbsp;FAILED: " + testResult.message);
      }
      this.output.write("<br />");      
    }
}
//}


//class ElementOutputStream{
var ElementOutputStream = function(element){
  this.element = element;
}
ElementOutputStream.prototype = {
  write: function(value) {
    this.element.innerHTML += value;
  },
  clear: function() {
    this.element.innerHTML = "";
  }
}
//}

//class Assert{
var Assert = new Object();
Assert.areEqual = function(expected, actual, message) {
  if (expected != actual) {
    throw new AssertionError("Expected: " + expected + ", got: " + actual + ". " + (message || ""));
  }
}
Assert.that = function(actualValue, constraint, message) {
  if (constraint == null) { 
    constraint = Is.True;
  }
  if (!constraint.match) { 
    throw new AssertionError("Constraint must have a match() method that takes the actual value as an argument.");
  }
  var result = constraint.match(actualValue);
  if (result instanceof AssertionError) { result.raiseException(message); }
}
//}

//class AssertionError{
var AssertionError = function(message) {
  this.name = "AssertionError";
  this.message = message;  
}
AssertionError.prototype = {
  raiseException: function(appendMessage) { 
    if (appendMessage) {
      this.message += " \n" + appendMessage;
    }
    throw this;
  }
}
//}

//class TrueConstraint{
var TrueConstraint = function(){}
TrueConstraint.prototype = {
  match: function(actualValue) {        
    if (actualValue != true) {
      return new AssertionError("Expected true, got: " + actualValue);
    }
  }
}
//}

//class FalseConstraint{
var FalseConstraint = function(){}
FalseConstraint.prototype = {
  match: function(actualValue) {
    if (actualValue != false) {
      return new AssertionError("Expected false, got: " + actualValue);
    }
  }
}
//}

//class InCollectionConstraint{
var InCollectionConstraint = function(collection) {
  this.collection = collection;
}
InCollectionConstraint.prototype = {
  comparer: null,
  match: function(actualValue) {
    if (this.collection.length == 0) {
      return new AssertionError("Collection had no values");    
    }
    for (var item in this.collection) {
      var result = new EqualToConstraint(this.collection[item], this.comparer).match(actualValue);
      if (! (result instanceof AssertionError) ) {
        return;
      }
    }
    return new AssertionError("Expected '" + actualValue + "' in collection.");
  },
  UsingComparer: function(comparer) { 
    this.comparer = comparer; 
    return this;
  }    
}
//}

//class EqualToConstraint{
var EqualToConstraint = function(expectedValue, customComparer) { 
  this.expectedValue = expectedValue;     
  this.comparer = (customComparer) ? customComparer : null;  
}
EqualToConstraint.prototype = {
  match: function(actualValue) {
    if (this.comparer != null) {
      if (!this.comparer(actualValue, this.expectedValue)) {
        return new AssertionError("Expected value to equal '" + this.expectedValue + 
                                  "', but was '" + actualValue + 
                                  "', using provided comparison.");
      }
      return;
    }
    
    if (actualValue == this.expectedValue) {
      return;      
    }
    if (actualValue instanceof Array && this.expectedValue instanceof Array) {
      if (actualValue.length != this.expectedValue.length) {
        return new AssertionError("Expected value to equal '" + this.expectedValue + "', but was '" + actualValue + "'. Array lengths different.");
      }
      for (var i=0; i<actualValue.length; i++) {
        if (actualValue[i] != this.expectedValue[i]) {
          return new AssertionError("Expected value to equal '" + this.expectedValue + "', but was '" + actualValue + "'. Arrays differ at index " + i + ".");
        }
      }
      return;
    }
    return new AssertionError("Expected value to equal '" + this.expectedValue + "', but was '" + actualValue + "'");
  }
}
//}

//class Is{
var Is = new Object();
Is.True = new TrueConstraint();
Is.False = new FalseConstraint();
Is.In = function(collection) { return new InCollectionConstraint(collection); };
Is.EqualTo = function(expectedValue) { return new EqualToConstraint(expectedValue); };
//}

