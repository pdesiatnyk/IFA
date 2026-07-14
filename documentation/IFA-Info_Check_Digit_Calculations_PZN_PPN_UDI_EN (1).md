# Technical Information regarding PZN Coding IFA INFORMATION logo

- Check Digit Calculations of PZN, PPN and Basic UDI-DI -

## Check digit calculation of the PZN

The check digit of the PZN is calculated based on mod 11. Each digit of the PZN is weighted with a different factor from one to nine. The sum is formed across the products and divided by 11. The whole number remainder is the check digit.

If the remainder is the number 10, this digit sequence is not used as PZN.

<u>Example – Formation of the check digit of a PZN:</u>

For the PZN with the digit sequence of “2758089”, the check digit “9” is calculated as follows:

<table>
  <thead>
    <tr>
        <th> </th>
        <th>1st digit</th>
        <th>2nd digit</th>
        <th>3rd digit</th>
        <th>4th digit</th>
        <th>5th digit</th>
        <th>6th digit</th>
        <th>7th digit</th>
        <th>check digit</th>
    </tr>
    <tr>
        <th>PZN</th>
        <th>2</th>
        <th>7</th>
        <th>5</th>
        <th>8</th>
        <th>0</th>
        <th>8</th>
        <th>9</th>
        <th>9</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>Weighting factor</td>
        <td>1</td>
        <td>2</td>
        <td>3</td>
        <td>4</td>
        <td>5</td>
        <td>6</td>
        <td>7</td>
        <td rowspan="2">▲</td>
    </tr>
    <tr>
        <td>Product from digit and weighting factor</td>
        <td>2</td>
        <td rowspan="3">14</td>
        <td>15</td>
        <td>32</td>
        <td>0</td>
        <td>48</td>
        <td>63</td>
    </tr>
    <tr>
        <td>Sum</td>
        <td colspan="7">174</td>
    </tr>
    <tr>
        <td>Division</td>
        <td colspan="7">174 / 11 = 15 Remainder 9 ►</td>
    </tr>
  </tbody>
</table>

As a result, the complete PZN is: 27580899

## Check digit calculation of the PPN and the Basic UDI-DI

The check digit of the PPN and the Basic UDI-DI is calculated based on mod 97. In the process, the decimal values of the ASCII table from 00 to 127 are assigned to the characters of the initial string of the PPN / Basic UDI-DI. Each digit of this string is weighted with a factor. The product of the ASCII decimal values are added up and divided by 97. As a numerical value, the remainder forms the two-digit check digit from 00 to 99. A one-digit remainder is filled with a leading zero. The remainder is adopted as a numerical value and not represented by the corresponding ASCII character.

Therefore, it is ensured that the check digit of the PPN and the Basic UDI-DI consists of digits only. As a result, numeric sequences also remain numeric.

The weighting of the digits starts on the left with two and increases by one for each following digit. This algorithm provides the check digit for both purely numeric and alphanumeric strings.

For additional information on the PZN and PPN plus Basic UDI-DI, see www.ifaffm.de

<page_number>Page 1 of 2</page_number>

26 January 2024    Informationsstelle für Arzneispezialitäten – IFA GmbH    [Top](Top)

Check Digit Calculations of PZN, PPN and Basic UDI-DI

IFA INFORMATION logo

<u>Example – Check digit generation for a PPN:</u>

For the German market, the PPN contains the 8-digit PZN with the preceding Product Registration Agency Code (PRA Code) “11”. For the PPN with PRA Code “11” and PZN “03752864”, the check digit is calculated as follows:

<table>
  <thead>
    <tr>
        <th> </th>
        <th colspan="2">PRA Code</th>
        <th colspan="8">PZN</th>
        <th colspan="2">check digits PPN</th>
    </tr>
    <tr>
        <th>PPN</th>
        <th>1</th>
        <th>1</th>
        <th>0</th>
        <th>3</th>
        <th>7</th>
        <th>5</th>
        <th>2</th>
        <th>8</th>
        <th>6</th>
        <th>4</th>
        <th>1</th>
        <th>4</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>ASCII character value</td>
        <td>49</td>
        <td>49</td>
        <td>48</td>
        <td>51</td>
        <td>55</td>
        <td>53</td>
        <td>50</td>
        <td>56</td>
        <td>54</td>
        <td>52</td>
        <td> </td>
        <td> </td>
    </tr>
    <tr>
        <td>Weighting factor</td>
        <td>2</td>
        <td>3</td>
        <td>4</td>
        <td>5</td>
        <td>6</td>
        <td>7</td>
        <td>8</td>
        <td>9</td>
        <td>10</td>
        <td>11</td>
        <td> </td>
        <td> </td>
    </tr>
    <tr>
        <td>Product from decimal value and weighting factor</td>
        <td>98</td>
        <td>147</td>
        <td>192</td>
        <td>255</td>
        <td>330</td>
        <td>371</td>
        <td>400</td>
        <td>504</td>
        <td>540</td>
        <td>572</td>
        <td> </td>
        <td> </td>
    </tr>
    <tr>
        <td>Sum</td>
        <td colspan="12">3409</td>
    </tr>
    <tr>
        <td>Division</td>
        <td colspan="12">3409 / 97 = 35 remainder 14</td>
    </tr>
  </tbody>
</table>

The check digit is built by the numeric remainder 14 and represents the last two digits of the PPN. As a result, this is the complete PPN: 110375286414

<u>Example - Check digit generation of a Basic UDI-DI:</u>

The Basic UDI-DI contains the elements IAC, Manufacturer Code and Device Group Code. For the exemplary used values, the check digit is calculated as follows:

<table>
  <thead>
    <tr>
        <th> </th>
        <th colspan="2">IAC</th>
        <th colspan="10">Manufacturer Code</th>
        <th colspan="11">Device Group Code</th>
        <th colspan="2">check digits</th>
    </tr>
    <tr>
        <th>Basic UDI-DI</th>
        <th>P</th>
        <th>P</th>
        <th>1</th>
        <th>2</th>
        <th>3</th>
        <th>4</th>
        <th>5</th>
        <th>A</th>
        <th>B</th>
        <th>C</th>
        <th>D</th>
        <th>.</th>
        <th>1</th>
        <th>2</th>
        <th>3</th>
        <th>4</th>
        <th>5</th>
        <th>6</th>
        <th>7</th>
        <th>8</th>
        <th>.</th>
        <th>9</th>
        <th>0</th>
        <th>0</th>
        <th>4</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>ASCII character value</td>
        <td>80</td>
        <td>80</td>
        <td>49</td>
        <td>50</td>
        <td>51</td>
        <td>52</td>
        <td>53</td>
        <td>65</td>
        <td>66</td>
        <td>67</td>
        <td>68</td>
        <td>46</td>
        <td>49</td>
        <td>50</td>
        <td>51</td>
        <td>52</td>
        <td>53</td>
        <td>54</td>
        <td>55</td>
        <td>56</td>
        <td>46</td>
        <td>57</td>
        <td>48</td>
        <td> </td>
        <td> </td>
    </tr>
    <tr>
        <td>Weighting factor</td>
        <td>2</td>
        <td>3</td>
        <td>4</td>
        <td>5</td>
        <td>6</td>
        <td>7</td>
        <td>8</td>
        <td>9</td>
        <td>10</td>
        <td>11</td>
        <td>12</td>
        <td>13</td>
        <td>14</td>
        <td>15</td>
        <td>16</td>
        <td>17</td>
        <td>18</td>
        <td>19</td>
        <td>20</td>
        <td>21</td>
        <td>22</td>
        <td>23</td>
        <td>24</td>
        <td> </td>
        <td> </td>
    </tr>
    <tr>
        <td>Product from decimal value and weighting factor</td>
        <td>160</td>
        <td>240</td>
        <td>196</td>
        <td>250</td>
        <td>306</td>
        <td>364</td>
        <td>424</td>
        <td>585</td>
        <td>660</td>
        <td>737</td>
        <td>816</td>
        <td>598</td>
        <td>686</td>
        <td>750</td>
        <td>816</td>
        <td>884</td>
        <td>954</td>
        <td>1026</td>
        <td>1100</td>
        <td>1176</td>
        <td>1012</td>
        <td>1311</td>
        <td>1152</td>
        <td> </td>
        <td> </td>
    </tr>
    <tr>
        <td>Sum</td>
        <td colspan="25">16203</td>
    </tr>
    <tr>
        <td>Division</td>
        <td colspan="25">16203 / 97 = 167 remainder 4</td>
    </tr>
  </tbody>
</table>

The check digit is built by the numeric remainder 4, expressed by “04”, and represents the last two digits of the Basic UDI-DI. As a result, this is the complete Basic UDI-DI: PP12345ABCD.12345678.9004

<page_number>Page 2 of 2</page_number>

26 January 2024 Informationsstelle für Arzneispezialitäten – IFA GmbH [Top](Top)