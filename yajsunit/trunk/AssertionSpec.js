var AssertionSpec = function() {}

AssertionSpec.shouldFail = function(assertionFunction, messageOnFail) {
  try {
    assertionFunction();        
  } catch (ex) {      
    if (ex instanceof AssertionError) {
      return;
    }    
  }
  throw new AssertionError("Expected AssertionError exception. " + messageOnFail);
}

AssertionSpec.prototype = {
  name: "Assertion spec",
  should_be_able_to_pass_in_constraint: function() {
    var constraint = { match: function(actualValue) { return true; } }
    Assert.that(true, constraint);
  },
  should_throw_if_non_constraint_object_given: function() {
    AssertionSpec.shouldFail(function() { Assert.that(false, new Object()); }, "Should fail due to invalid constraint.");
  },
  should_use_true_constraint_if_no_constraint_given: function() {
    Assert.that(1==1);
    Assert.that(true, null);
    AssertionSpec.shouldFail(function() { Assert.that(false); }, "Should fail when using default constraint");
  }
}

var TrueConstraintSpec = function() {}
TrueConstraintSpec.prototype = {
  name: "TrueConstraint spec",
  should_pass_with_true:
    function() {
      Assert.that(true, Is.True);
    },
  should_fail_with_false:
    function() {
      AssertionSpec.shouldFail(function() { Assert.that(false, Is.True); }, "Expected 'false' to fail Is.true constaint");
    },
  should_fail_with_null:
    function() {
      AssertionSpec.shouldFail(function() { Assert.that(null, Is.True); }, "Expected 'null' to fail Is.true constaint");
    },
  should_fail_with_object:
    function() {
      AssertionSpec.shouldFail(function() { Assert.that(new Object(), Is.True); }, "Expected 'Object' to fail Is.true constaint");
    }
}

var FalseConstraintSpec = function() {}
FalseConstraintSpec.prototype = {  
  name: "FalseConstraint spec",
  should_pass_with_false:
    function() {
      Assert.that(false, Is.False);
    },
  should_fail_with_true:
    function() {
      AssertionSpec.shouldFail(function() { Assert.that(true, Is.False); }, "Expected 'true' to fail Is.False constaint");
    },
  should_fail_with_null:
    function() {
      AssertionSpec.shouldFail(function() { Assert.that(null, Is.False); }, "Expected 'null' to fail Is.False constaint");
    },
  should_fail_with_object:
    function() {
      AssertionSpec.shouldFail(function() { Assert.that(new Object(), Is.False); }, "Expected 'Object' to fail Is.False constaint");
    }
}

var InCollectionConstraintSpec = function() {}
InCollectionConstraintSpec.prototype = {  
  name: "InCollectionConstraint spec",
  should_fail_if_collection_is_empty: function() {
    AssertionSpec.shouldFail(function() { Assert.that(1, Is.In([])); }, "Expected empty collection to always fail Is.In constraint");
  },
  should_fail_if_not_in_collection: function() {
    AssertionSpec.shouldFail(function() { Assert.that(1, Is.In([2,3,4,5])); }, "Expected 1 to fail Is.In for [2,3,4,5]");
  },
  should_pass_when_value_in_collection: function() {
    Assert.that(1, Is.In([2,3,1,4,5]));
  },
  should_pass_when_value_in_mixed_type_collection: function() {
    Assert.that("abc", Is.In( { Field0: 1, Field1: "Test", Field2: "abc", Field3: new Object() }));
  },
  should_pass_when_array_is_in_array_of_arrays: function() {
    Assert.that([1,2], Is.In( [ [1,2,3], [1,2], [2,4,5] ]) );
  },
  should_be_able_to_use_provided_comparer: function() {
    var someObject = { someField: "test" };
    Assert.that(someObject, 
                Is.In([1, 2, 3, 4]).UsingComparer(function(first, second) { return second==3; })
                );
    AssertionSpec.shouldFail(
      function() {
          Assert.that(someObject, 
                Is.In([1, 2, 3, 4]).UsingComparer(function(first, second) { return second==5; })
                );
        }
    );
  }
}

var EqualToConstraintSpec = function() {}
EqualToConstraintSpec.prototype = {
  name: "EqualToConstraint spec",
  should_pass_when_primitive_value_matches: function() {
    Assert.that(2, Is.EqualTo(2));
    Assert.that("Hello, World!", Is.EqualTo("Hello, World!"));
  },
  should_fail_when_primitive_value_does_not_match: function() {
    AssertionSpec.shouldFail(function() { Assert.that(1, Is.EqualTo(2)); }, "Expected 1 to fail Is.EqualTo(2)");
  },
  should_pass_with_equal_arrays: function() {
    Assert.that([2, "abc", 5], Is.EqualTo([2, "abc", 5]));
  },
  should_pass_when_primitives_are_equal_with_standard_equality_operator: function() {
    Assert.that("210", Is.EqualTo(210));
    Assert.that(true, Is.EqualTo(1));
  },
  should_fail_with_arrays_with_different_items: function() {
    AssertionSpec.shouldFail(
      function() { Assert.that([2, "c", 4], Is.EqualTo([2, "abc", 5])); }, 
      "Expected [2,'c',4] not equal to [2,'abc', 5]"
    );
  }  
}