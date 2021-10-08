# Pascal compilier
## Syntax for this compilier (in BNF)
<details>
  <summary>Syntax</summary>

```
<program> ::= program <identifier>; <block> . 

<identifier> ::= <letter > {<letter or digit>}

<letter or digit> ::= <letter> | <digit>

<block> ::= <constant definition part> 

<type definition part> <variable declaration part>

<procedure and function declaration part> <statement part>

<constant definition part> ::= <empty> | const <constant definition> 
{ ; <constant definition>} ;

<constant definition> ::= <identifier> = <constant> 

<constant> ::= <unsigned number> | <sign> <unsigned number> | 
<constant identifier> | <sign> <constant identifier> | <string>

<unsigned number> ::= <unsigned integer> | <unsigned real>

<unsigned integer> ::= <digit> {<digit>}

<unsigned real> ::= <unsigned integer> . <unsigned integer> | 

<unsigned integer> . <unsigned integer> E <scale factor> |
<unsigned integer> E <scale factor>

<scale factor> ::= <unsigned integer> | <sign> <unsigned integer>

<sign> ::= + | -

<constant identifier> ::= <identifier>

<string> ::= '<character> {<character>}'

<type> ::= <simple type> | <structured type> | <pointer type>

<simple type> ::= <scalar type> | <subrange type> | <type identifier>

<scalar type> ::= (<identifier> {,<identifier>})

<subrange type> ::= <constant> .. <constant>

<type identifier> ::= <identifier>

<structured type> ::= <array type> | <record type> 

<array type> ::= array [<index type>{,<index type>}] of <component type>

<index type> ::= <simple type>

<component type> ::= <type>

<record type> ::= record <field list> end

<field list> ::= <fixed part> | <fixed part> ; <variant part> | <variant part>

<fixed part> ::= <record section> {;<record section>}

<record section> ::= <field identifier> {, <field identifier>} : <type> | <empty>

<base type> ::= <simple type>

<pointer type> ::= <type identifier>

<variable declaration part> ::= <empty> | var <variable declaration> {; <variable declaration>} ;

<variable declaration> ::= <identifier> {,<identifier>} : <type>

<procedure and function declaration part> ::= {<procedure or function declaration > ;}

<procedure or function declaration > ::= <procedure declaration > |
<function declaration >

<procedure declaration> ::= <procedure heading> <block>

<procedure heading> ::= procedure <identifier> ; |

procedure <identifier> ( <formal parameter section> {;<formal parameter section>} );

<formal parameter section> ::= <parameter group> | var <parameter group> |

function <parameter group> | procedure <identifier> { , <identifier>}

<parameter group> ::= <identifier> {, <identifier>} : <type identifier>

<function declaration> ::= <function heading> <block>

<function heading> ::= function <identifier> : <result type> ; |

function <identifier> ( <formal parameter section> {;<formal parameter section>} ) : <result type> ;

<result type> ::= <type identifier>

<statement part> ::= <compund statement>

<statement> ::= <unlabelled statement>

<assignment statement> ::= <variable> := <expression> | <function identifier> := <expression>

<variable> ::= <entire variable> | <component variable> | <referenced variable>

<entire variable> ::= <variable identifier>

<variable identifier> ::= <identifier>

<component variable> ::= <indexed variable> | <field designator> 

<indexed variable> ::= <array variable> [<expression> {, <expression>}]

<array variable> ::= <variable>

<field designator> ::= <record variable> . <field identifier>

<record variable> ::= <variable>

<field identifier> ::= <identifier>

<referenced variable> ::= <pointer variable>

<pointer variable> ::= <variable>

<expression> ::= <simple expression> | <simple expression> <relational operator> <simple expression>

<relational operator> ::= = | <> | < | <= | >= | > | in

<simple expression> ::= <term> | <sign> <term>| <simple expression> <adding operator> <term>

<adding operator> ::= + | - | or

<term> ::= <factor> | <term> <multiplying operator> <factor>

<multiplying operator> ::= * | / | div | mod | and

<factor> ::= <variable> | <unsigned constant> | ( <expression> ) | <function designator> | <set> | not <factor>

<unsigned constant> ::= <unsigned number> | <string> | < constant identifier> < nil>

<function designator> ::= <function identifier> | <function identifier ( <actual parameter> {, <actual parameter>} )

<function identifier> ::= <identifier>

<procedure statement> ::= <procedure identifier> | <procedure identifier> (<actual parameter> {, <actual parameter> })

<procedure identifier> ::= <identifier>

<actual parameter> ::= <expression> | <variable> | <procedure identifier> | <function identifier>

<empty statement> ::= <empty>

<empty> ::=

<structured statement> ::= <compound statement> | <conditional statement> | <repetitive statement> | <with statement>

<compound statement> ::= begin <statement> {; <statement> } end;

<conditional statement> ::= <if statement> | <case statement>

<if statement> ::= if <expression> then <statement> | if <expression> then <statement> else <statement>

<repetitive statement> ::= <while statement> | <repeat statemant> | <for statement>

<while statement> ::= while <expression> do <statement>

<repeat statement> ::= repeat <statement> {; <statement>} until <expression>

<for statement> ::= for <control variable> := <for list> do <statement>

<control variable> ::= <identifier>

<for list> ::= <initial value> to <final value> | <initial value> downto <final value>

<initial value> ::= <expression>

<final value> ::= <expression>
```
</details>

