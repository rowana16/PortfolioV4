$(document).ready(function () {
    $("#myBtn").click(function () { $("#myModal").modal(); });

    $("#calculate").on("click", ".btn", function () {
        // initialize variables
        var max = 0;
        var min = 100;
        var count = 0;
        var sum = 0;
        var product = 1;

        //loop through each input
        for (var i = 1; i <11; i++){
            // get values
            var currValue = $("#input" + i).val()
            // ensure inputs are valid
            if (!isNaN(parseInt(currValue)) && parseInt(currValue)<=100 && parseInt(currValue) > 0) {
                value = parseInt(currValue);
                /*
                Debugging Tool: alert(i + ": value - " + value + "  Max: " + max + "  Min: " + min + "  Sum: " + sum + "  count: " + count + "  product: " + product); 
                */
               // Additional Error Handling 
                if (value != "") {
                    max = maxCompare(max, value);
                    min = minCompare(min, value);
                    count++;
                    sum = sum + value;
                    product = product * value;
                }
            }

        }

        $("#max").text(max);
        $("#min").text(min);
        $("#ave").text(sum / count);
        $('#sum').text(sum);
        $('#product').text(product);
    })

    //simple max and min functions
    function maxCompare(currentMax, newValue) {
        if (currentMax >= newValue) { return currentMax; }
        else if (currentMax < newValue) { return newValue; }     
    }

    function minCompare(currentMin, newValue) {
        if (currentMin <= newValue) { return currentMin; }
        else if (currentMin > newValue) { return newValue; }     
    }

    /********************** Buttons ******************************/
    /*** Factorial ***/
    $("#factorial").click(function () {
        $("#factorialModal").modal();
    });

    /*** FizzBuzz ***/
    $("#fizzBuzz").click(function () {
        $("#fizzModal").modal();
    });

    /*** Palindrome ***/
    $("#palindrome").click(function () {
        $("#palinModal").modal();
    });

    /********************** Functions ******************************/
    /*** Factorial ***/
    $('#calculate').click(function (e) {
        e.preventDefault();
        var value = $('#factorial').val();

        if (value > 0) {
            var output = 1;

            for (var i = 1; i <= value; i++) {
                output *= i;
            }

            $('#result').text("Result: " + output);
            $('#factorial').val("");
        }
    });

    /*** FizzBuzz ***/
    $('#calcFizzBuzz').click(function (e) {
        e.preventDefault();
        var calc1 = $('#fizz').val();
        var calc2 = $('#buzz').val();
        $('#fizzResult').text(' ');

        fizzBuzz(calc1, calc2);

        $('#fizz').val(' ');
        $('#buzz').val(' ');
    });

    function checkInt(input) {
        //console.log("Check Int");
        input = Math.ceil(input);
        //console.log(input);
        input = parseInt(input);
        //console.log(input);
        if (isNaN(input)) {
            //console.log("NaN");
            alert("Not a Valid Integer");
        }
        else { return input; }
    }

    function fizzBuzz(input1, input2) {
        var denom1 = checkInt(input1);
        var denom2 = checkInt(input2);

        $('#fizzValue').text('Fizz:  ' + denom1);
        $('#buzzValue').text('Buzz:  ' + denom2);
        for (var count = 1; count <= 100; count++) {
            var output;
            if (count % denom1 === 0 && count % denom2 === 0)
            { output = "<span style='color:red; font-weight:bold;'>FizzBuzz</span>"; }
            else if (count % denom1 === 0) { output = "<span style='color:cornflowerblue; font-weight:bold;'>Fizz</span>"; }
            else if (count % denom2 === 0) { output = "<span style='color:darkolivegreen; font-weight:bold;'>Buzz</span>"; }
            else { output = count; }

            if (count % 5 === 0) { $('#fizzResult').append(output + '<br/>'); }
            else { $('#fizzResult').append('<span style="margin:15px;">' + output + '</span>'); }
        }
    }

    /*** Palindrome ***/

    $('#calcPalindrome').click(function (e) {
        e.preventDefault();
        $('#palinResult').text('Result: ');
        var evalString = $('#palinIn').val();
        evalString = evalString.toUpperCase();
        evalString = evalString.replace(/[^A-Z0-9]/g, '');
        //evalString = evalString.replace(/^\w/g, "");

        for (var i = 1; i <= evalString.length; i++) {
            if (evalString.substring(i - 1, i) != evalString.substring(evalString.length - i, evalString.length - i + 1)) {
                $('#palinResult')
                    .append("Not a Palindrome")
                    .attr('class', 'label label-warning');
                break;
            }

            if (i === evalString.length) {
                $('#palinResult')
                    .append("It's a Palindrome!!!")
                    .attr('class', 'label label-success');
            }
        }
        $('#palinIn').val('');
    });

    
}); //doc ready