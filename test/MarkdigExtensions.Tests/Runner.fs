module Runner

open Expecto

[<EntryPoint>]
let main args =
    let tests = testList "All" [ UrlRewriterTests.tests; ImageAsFigureTests.tests ]
    Tests.runTestsWithArgs defaultConfig args tests
