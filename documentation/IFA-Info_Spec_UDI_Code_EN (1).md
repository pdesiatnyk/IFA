# IFA Coding System

## Specification
## Unique Device Identification (UDI)

Use of the IFA Coding System for medical devices
in accordance with Regulations (EU) 2017/745 and (EU) 2017/746

Supplement for manufacturers of medical devices

UDI logo

IFA CODINGSYSTEM logo

IFA Coding System for medical devices IFA CODINGSYSTEM logo

# Directory

1. Quick Access 4
2. Introduction 5
3. Unique Device Identification (UDI) for medical devices 6
3.1. General 6
3.2. Device Identifier – UDI-DI 6
3.2.1. UDI-DI with PPN containing PZN 7
3.2.2. UDI-DI with HPC for manufacturer’s item number/product reference 8
3.2.2.1. Structure of the HPC 8
3.2.2.2. Details on the packaging level index 10
3.3. Production Identifier – UDI-PI 10
3.4. Master UDI-DI 10
3.4.1. Structure of the Master UDI-DI 11
3.4.2. Data string of the Master UDI-DI 12
3.4.3. HRI format “Interpretation Line” 12
3.5. Basic UDI-DI 13
3.6. Data content and requirements for the Data Matrix 15
3.7. Additional data elements 15
4. Marking with code and plain text format 16
4.1. Coding 16
4.1.1. Direct Marking 16
4.1.2. Test Requirements of ISO/IEC 29158 19
4.1.3. Marking of packaging levels 20
4.1.4. Parameters and quality requirements for medical device packaging 21
4.2. UDI-Labelling in plain text format 21
4.2.1. General information 21
4.2.2. Marking with PZN 22
4.2.3. HRI format “Symbol” 22
4.2.4. HRI format “Symbol +” 23
4.2.5. HRI format “Interpretation Line” 24
4.2.6. Peculiarities 25
4.3. HRI format for documentation and records 25
4.3.1. XML format 26
4.3.2. Data Identifier Format 26
4.4. Emblem for the Data Matrix Code 26
5. EUDAMED 27

Version 2.5 <page_number>Page 2 of 40</page_number>

1 April 2026 Informationsstelle für Arzneispezialitäten – IFA GmbH <u>Directory</u>

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

6. Examples of UDI marking on medical devices 28
6.1. Example 1 – Medical device without separate PZN 28
6.2. Example 2 – Medical device software for download 29
6.3. Example 3 – Medical device software download + DVD 30
6.4. Example 4 – Batch-related medical device 31
6.5. Example 5 – Medical device with UDI and PZN in Code 39 31
6.6. Example 6 – Medical device with URL in the Data Matrix 32
6.7. Example 7 – Serialized medical device 32
7. Examples for UDI marking with HPC 33
7.1. Example 8 – Medical device with HPC 33
7.2. Example 9 – Medical device with HPC coding DIN 16598 34
7.3. Example 10 – HPC with various packaging levels 35
8. Examples for UDI marking with Master UDI-DI 37
8.1. Example 11 – Medical device with PPN and Master UDI-DI 37
8.2. Example 12 – Medical device with HPC and Master UDI-DI 37
9. Appendix A: Overview and reference of the data identifiers 38
10. Appendix B: Dokument history 40

Version 2.5

<page_number>Page 3 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

<u>Directory</u>

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

# 1. Quick Access

<table>
  <thead>
    <tr>
        <th colspan="2">Basic UDI-DI</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>Generate BUDI</td>
        <td>* Use the BUDI generator<sup>1</sup> in the IFA portal<sup>2</sup> to generate the Basic UDI-DI.<br/>* See chapter 3.5. for details.</td>
    </tr>
  </tbody>
</table>
<table>
  <thead>
    <tr>
        <th colspan="2">UDI-DI</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td colspan="2">3 options on how to obtain UDI-DI</td>
    </tr>
    <tr>
        <td>PPN allocation<sup>3</sup></td>
        <td>* Order in IFA portal.<br/>* Or download the order file A – PZN Pre-allocation on the IFA website and fill in the file.<br/>Send the completed file by email to ifa@ifaffm.de.<br/>* IFA sends the allocated PPN in the order confirmation by email.</td>
    </tr>
    <tr>
        <td>PPN including PZN publication via IFA Information Services</td>
        <td>* Order in IFA portal.<br/>* Or download the order file B3 – First Publication Medical Device on the IFA website and fill in the file.<br/>Send the completed file including product information by email to ifa@ifaffm.de.<br/>* IFA sends the allocated PPN in the order confirmation by email.</td>
    </tr>
    <tr>
        <td>Generate HPC</td>
        <td>* Use the HPC generator<sup>4</sup> in IFA portal to construct HPC.<br/>* See chapter 3.2.2.1. for details.</td>
    </tr>
  </tbody>
</table>

Figure 1: Quick Access

<sup>1</sup> [https://www.ifaffm.de/en/ifa-codingsystem/udi/budi-generator.html](https://www.ifaffm.de/en/ifa-codingsystem/udi/budi-generator.html)

<sup>2</sup> [https://www.ifaffm.de/en/ifa-suppliers/ifa-portal.html](https://www.ifaffm.de/en/ifa-suppliers/ifa-portal.html)

<sup>3</sup> [https://www.ifaffm.de/en/ifa-codingsystem/udi/issuing_udi-di.html](https://www.ifaffm.de/en/ifa-codingsystem/udi/issuing_udi-di.html)

<sup>4</sup> [https://www.ifaffm.de/en/ifa-codingsystem/udi/hpc-generator.html](https://www.ifaffm.de/en/ifa-codingsystem/udi/hpc-generator.html)

Version 2.5

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

Page 4 of 40

[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

# 2. Introduction

This specification supplements the IFA specification PPN-Code Specification for Retail Packaging<sup>5</sup>, focussing on the requirements that must be met in accordance with Regulations (EU) 2017/745 (MDR) and (EU) 2017/746 (IVDR). This specification references to the respective chapter of the IFA specifications PPN-Code Specification for Retail Packaging where appropriate.

UDI logo

Informationsstelle für Arzneispezialitäten – IFA GmbH (IFA) is accredited as an Issuing Entity pursuant to ISO/IEC 15459-2 and facilitates the application of the Pharmazentralnummer (PZN) according to international standards by using the Pharmacy Product Number (PPN). The Health Product Code (HPC) provides for the option to apply various product references for UDI using the IFA Coding System without product notification in the IFA database. The IFA Coding System is already successfully being used in the field of medicinal products during the implementation of the EU Falsified Medicines Directive. With COMMISSION IMPLEMENTING DECISION (EU) 2019/939 of 6 June 2019, the Commission designated IFA as Issuing Entity for UDI. Thus, the UDI requirements set out in the European Medical Device Regulation (MDR) and the In Vitro Diagnostic Medical Devices Regulation (IVDR) can be satisfied using the IFA Coding System.

<sup>5</sup> [https://www.ifaffm.de/mandanten/1/documents/04_ifa_coding_system/IFA_Spec_PPN_Code_Handelspackung_EN.pdf](https://www.ifaffm.de/mandanten/1/documents/04_ifa_coding_system/IFA_Spec_PPN_Code_Handelspackung_EN.pdf)

Version 2.5

Page 5 of 40

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

# 3. Unique Device Identification (UDI) for medical devices

## 3.1. General

MDR and IVDR introduce the UDI system as an identification system for medical devices. The main features are the Device Identifier UDI-DI, the Production Identifier UDI-PI, the Basic UDI-DI and the device registration in the EUDAMED database (EUDAMED).

The Device Identifier UDI-DI is a numeric or alphanumeric code relating to a medical device. For this purpose PPN or HPC can be utilized. Various packaging levels with different pack sizes require for separate UDI-DI, whereas shipping containers are exempted.

The Production Identifier UDI-PI is a manufacturing characteristic identifying the unit of device production (batch/lot/manufacturing date/expiry date).

The data elements provided for the coding of UDI-DI and UDI-PI are referred to as UDI in Section 1 Part C Annex VI MDR. UDI-DI and UDI-PI are placed on the label and in case of reusable devices on the device itself as a machine-readable code (AIDC) and additionally in a human readable form (HRI). The product marking itself is described as UDI carrier in Section 4.1 Part C Annex VI MDR.

The Basic UDI-DI is the main key for product groups of a manufacturer sharing the same core characteristics. It is the main access key for device-related vigilance-information in EUDAMED and is neither placed on the label nor the device.

EUDAMED is the medical device data base hosted by the commission.

<mark>The data elements required to implement the UDI-rules arising from MDR and IVDR can be generated via the IFA Coding System. For manufacturers using the PZN for product identification already, IFA provides its Coding System and the PPN without additional licencing costs.</mark>

General information about the MDR can be found here.

Guidance to implement the MDR requirements provide the following documents:

* MDCG 2019-15 GUIDANCE NOTES FOR MANUFACTURERS OF CLASS I MEDICAL DEVICES

* MDCG 2021-19 Guidance note integration of the UDI within an organisation’s quality management system

The following sections provide a more detailed description of the UDI data elements and their generation. Details for the other data elements and their coding can be found in the IFA specifications PPN-Code Specification for Retail Packaging. In the examples in chapter 6 as well as in Appendix A for lot number the term batch number is used as a term required by regulation on medicinal products.

## 3.2. Device Identifier – UDI-DI

The IFA Coding System offers the PPN in two formats, both of which can be used as UDI-DI. One is the PPN with the prefix "11", which contains PZN and is described in more detail in chapter 3.2.1. Secondly, the prefix "13" offers the possibility to present product references of the manufacturer in this PPN-format. Both variants are determined in the data string of the code with the data identifier 9N, which is registered for the PPN. To allow for a clear distinction, the PPN with the prefix "13" is referred to as Health Product Code (HPC). The structure of the HPC is shown in chapter 3.2.2.

Version 2.5

<page_number>Page 6 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[<u>Directory</u>](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

# 3.2.1. UDI-DI with PPN containing PZN

The PPN represents the PZN with the prefix "11" in an internationally unambiguous format:

**PPN (Pharmacy Product Number)**
<span style="color: #E36C09; font-size: 2em;">11</span> <span style="color: #0070C0; font-size: 2em;">12345678</span> <span style="color: #C00000; font-size: 2em;">42</span>

<table>
  <thead>
    <tr>
        <th colspan="3">PPN (Pharmacy Product Number)</th>
    </tr>
    <tr>
        <th>PRA-Code</th>
        <th>PZN</th>
        <th>CC</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>11</td>
        <td>12345678</td>
        <td>42</td>
    </tr>
  </tbody>
</table>

* orange square **PRA-Code**: Product Registration Agency Code for PPN
* blue square **PZN**: Pharmazentralnummer
* red square **CC**: Check Character

Figure 2: Structure of the PPN

The PPN containing a PZN consist of three parts. The "11" stands for the Product Registration Agency Code (PRA Code) which IFA has assigned for the national item number PZN. The PZN follows after the "11" (8 digits). The subsequent digits form the two-digit, calculated check digit of the PPN across the entire data field (including the "11"). With the PZN shown in this example, the value "42" is resulting.

PPN containing a German product number PZN are in this PPN-format internationally unique and can be used for UDI-DI in the EU.

IFA issues PPN directly when assigning PZN, thus manufacturers do not need to generate PPN on their own. PPN can be found in the IFA order confirmation or can be requested free of charge at any time as product range file<sup>6</sup> in IFA portal.

For products not available on the German pharmacy market, PPN containing PZN can be assigned only. This means IFA assigns the PPN containing PZN to manufacturers without publishing the master data of the product through the IFA information services in the German healthcare system. PPN containing assigned-only PZN are therefore not disclosed to actors in the German healthcare system.

In the data string of the Data Matrix Code (Data Matrix), the PPN is represented with the data identifier "9N" (see Appendix A). During coding, the ASC data structure (Format 06) must be applied in accordance with subsection A chapter 5.1 IFA specification PPN-Code Specification for Retail Packaging. The complete structure of a data string with control characters is shown below in chapter 3.6. The data elements in a data string including a PPN which contains a PZN are:

* <9N> Data Identifier PPN
* <11> PRA-Code for PZN
* <PZN> 8 digit PZN
* <CC> PPN-Check digits: 2 digits Modulo 97

...

Further data elements follow, because the UDI-DI is encoded in the Data Matrix together with the UDI-PI per syntax ISO/IEC 15434. Details are described below in chapter 3.7.

<sup>6</sup> [https://www.ifaffm.de/en/ifa-suppliers/requirements-ead-file.html](https://www.ifaffm.de/en/ifa-suppliers/requirements-ead-file.html)

Version 2.5
1 April 2026
Informationsstelle für Arzneispezialitäten — IFA GmbH
Page <page_number>7</page_number> of 40
[Directory](https://www.ifaffm.de/en/ifa-suppliers/directory.html)

IFA Coding System for medical devices IFA CODINGSYSTEM logo

# 3.2.2. UDI-DI with HPC for manufacturer’s item number/product reference

Products which are not made available in the German pharmacy market can be provided with the HPC for UDI-DI.

Unlike the PPN, the HPC does not contain a PZN, but a product reference and a packaging level index of the manufacturer. Another difference is that the manufacturer generates the HPC on its own. Like assigned-only PPNs, the HPC is not published through the IFA information services (see chapter 3.2.1.), thus the HPC is not provided for the German pharmacy market.

## 3.2.2.1. Structure of the HPC

Manufacturers can use the HPC to implement their item references and a packaging level index in a UDI-DI, details in chapter 3.2.2.2.

**HPC (Health Product Code)**

<table>
  <thead>
    <tr>
        <th colspan="5">13 12345 MAX18 0 76</th>
    </tr>
    <tr>
        <th>PRA-Code</th>
        <th>CIN</th>
        <th>Supplier Part Number</th>
        <th>PLI</th>
        <th>CC</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>orange square PRA-Code</td>
        <td> </td>
        <td>Product Registration Agency Code for HPC</td>
        <td> </td>
        <td> </td>
    </tr>
    <tr>
        <td>dark grey square CIN</td>
        <td> </td>
        <td>IFA assigned CIN</td>
        <td> </td>
        <td> </td>
    </tr>
    <tr>
        <td>light green square Supplier Part Number</td>
        <td> </td>
        <td>Supplier assigned Part Number</td>
        <td> </td>
        <td> </td>
    </tr>
    <tr>
        <td>dark green square PLI</td>
        <td> </td>
        <td>Packaging level index (0-9)</td>
        <td> </td>
        <td> </td>
    </tr>
    <tr>
        <td>red square CC</td>
        <td> </td>
        <td>Check Character</td>
        <td> </td>
        <td> </td>
    </tr>
  </tbody>
</table>

Figure 3: Structure of the HPC

The HPC consists of the 5 elements shown above. The "13" stands for the Product Registration Agency Code of the HPC. The 5-digit IFA Supplier Number, which IFA assigns to manufacturers, is used as the manufacturer identification code CIN. It can be found in the first IFA order confirmation a new customer receives or in the summary of manufacturer address data<sup>7</sup>, which can be requested in IFA portal. This is followed by the manufacturer's 18-digit product number or reference number, which may be numeric or alphanumeric, but may not contain lowercase letters and may only contain a period (.) or hyphen (-) as a separator. If a 4-digit, exclusively numeric supplier part number is used, this results in HPC with 14 digits. Users may confuse these with the UDI-DI from another coding system. After that, the packaging level index with the values "0" to "8" follows. The meaning of this packaging level index is detailed in chapter 3.2.2.2. The following digits form the two-digit check digit, calculated over the complete data field including the "13" according to Modulo 97. To generate HPC use the HPC generator in IFA portal.

<sup>7</sup> [https://www.ifaffm.de/en/ifa-suppliers/requirements-ead-file.html](https://www.ifaffm.de/en/ifa-suppliers/requirements-ead-file.html)

Version 2.5 <page_number>Page 8 of 40</page_number>
1 April 2026 Informationsstelle für Arzneispezialitäten – IFA GmbH [Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

The HPC is specified as follows:

<table>
  <thead>
    <tr>
        <th colspan="6">HPC</th>
    </tr>
    <tr>
        <th>Substring element:</th>
        <th>PRA-Code</th>
        <th>CIN</th>
        <th>Supplier Assigned Part Number (Ref.-Nr. / Artikelnr.)</th>
        <th>Packaging level index</th>
        <th>Check Digits</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>generated by:</td>
        <td>IFA</td>
        <td>IFA</td>
        <td>Manufacturer</td>
        <td>Manufacturer</td>
        <td>Modulo 97</td>
    </tr>
    <tr>
        <td>Data type:</td>
        <td>A</td>
        <td>A/Num<sup>8</sup></td>
        <td>A/Num</td>
        <td>Num</td>
        <td>Num</td>
    </tr>
    <tr>
        <td>Character set:<sup>9</sup></td>
        <td>13</td>
        <td>0 – 9; A – Z</td>
        <td>0 – 9; A – Z; “.”;“-”</td>
        <td>0 – 9</td>
        <td>0 – 9</td>
    </tr>
    <tr>
        <td>Character length:</td>
        <td>2</td>
        <td>5</td>
        <td>18</td>
        <td>1</td>
        <td>2</td>
    </tr>
    <tr>
        <td>String length:</td>
        <td colspan="5">11 - ... - 28</td>
    </tr>
    <tr>
        <td>Example:</td>
        <td>13</td>
        <td>12345</td>
        <td>ABCD12345678</td>
        <td>0</td>
        <td>56</td>
    </tr>
  </tbody>
</table>

Figure 4: Specification of the HPC

In the data string of the Data Matrix, the HPC is indicated with the Data Identifier "9N" (see Appendix A). For coding, the ASC data structure (format 06) applies according to subitem A in the IFA specification PPN-Code Specification for Retail Packaging, chapter 5.1. The following sequence of HPC data elements results in the data string:

* <9N> Data Identifier PPN

* <13> PRA-Code for HPC

* <CIN> 5 digit IFA Supplier Number

* <Supplier Part Number> Product reference number of the manufacturer

* <PLI> Packaging level index of the manufacturer

* <CC> 2 digit check digit Modulo 97

...

Further data elements follow, because the UDI-DI is encoded in the Data Matrix together with the UDI-PI per syntax ISO/IEC 15434 or for HPC alternatively keyboard compatible per syntax DIN 16958. Details are described below in chapter 3.7..

<sup>8</sup> At present, exclusively numeric manufacturer codes are assigned.

<sup>9</sup> Corresponding ASCII characters: 48 – 57 for digits 0 – 9; 65 – 90 for characters A – Z; 45 for the “hyphen”; 46 for the “period”.

Version 2.5
1 April 2026
Informationsstelle für Arzneispezialitäten – IFA GmbH
Page <page_number>9</page_number> of 40
[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

## 3.2.2.2. Details on the packaging level index

The packing level index is placed as a one-digit number with the values 0 to 8 after the manufacturer’s item/product reference and before the check digits. The index allows to use the same item number for different packaging levels. In a UDI, the index is one of the UDI-DI data elements. The definition is up to the manufacturer, examples are:

* “0“: Device without packaging (Unit of Use; direct marking)

* “1“: Single packaging

* “2“: Packaging of 5 units

* “3“: Packaging of 50, etc. up to „8“

* “9” is reserved for variable quantities and cannot be used for UDI.

HPC generated without a packaging level index in accordance with version 1.04 of this specification remain valid, but must be provided with a packaging level index in case of a UDI-DI change.

See also example 10 below in chapter 7.3.

## 3.3. Production Identifier – UDI-PI

Depending on the requirement for a medical device, the manufacturer determines the UDI-PI for his product and labels the packages accordingly. The UDI-PI can be the lot number (batch number), expiry date and, in certain cases, also the manufacturing date, a serial number assigned by the manufacturer or several of these data elements. This also applies to reusable medical devices that are to be refurbished. For these data elements, the data identifiers pursuant to the international standard ANSI MH10.8.2. are used. The common data elements and data identifiers are described in Appendix A. Further information can be found in chapter 5.2.2. of the IFA specification PPN-Code Specification for Retail Packaging. Unlike the required randomization of serial numbers for medicinal products the design of serial numbers for UDI is in the sole responsibility of the manufacturer.

## 3.4. Master UDI-DI

In accordance with Art. 27 para 10 subpara b) MDR, the Commission adopted Delegated Regulation (EU) 2023/2197 requiring a Master UDI-DI for standard contact lenses and made to order contact lenses. Delegated Regulation (EU) 2025/788 amends its date of application by 1 year, until 9th November 2026.

For spectacle frames, spectacle lenses and ready-to-wear spectacles the Commission adopted Delegated Regulation (EU) 2025/1920 in order to apply the Master UDI solution on such kind of devices as from 1st November 2028.

The Master UDI-DI must not be used for other devices.

Find instructions on Master UDI-DI assignment, labelling, EUDAMED device registration and vigilance in MDCG 2024-14 Guidance on the implementation of the Master UDI-DI solution for contact lenses as well as in MDCG 2025-8 Guidance on the implementation of the Master UDI-solution for spectable frames, spectacle lenses and ready-to-wear reading spectacles.

See also MDCG 2025-7 Position Paper Timelines of the implementation of ‘Master UDI-DI’ to contact lenses and spectacle frames, spectacle lenses and ready-to-wear reading spectacles.

Version 2.5

<page_number>Page 10 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

Following Delegated Regulations (EU) 2023/2197 and (EU) 2025/1920, under the new section 6.6 for highly individualised devices in Part C of Annex VI to Regulation 2017/745, the UDI-DI for concerned devices is to be called Master UDI-DI and it will be assigned to a group of devices. The Master UDI-DI describes a product group.

The payload data elements of a UDI with a Master UDI-DI are therefore the following:

<table>
  <thead>
    <tr>
        <th colspan="3">UDI</th>
    </tr>
    <tr>
        <th>UDI-DI</th>
        <th>Master UDI-DI</th>
        <th>UDI-PI</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>PPN/HPC</td>
        <td>MUDI</td>
        <td>Lot/MNF Date/Serial</td>
    </tr>
  </tbody>
</table>

Figure 5: Data elements of a UDI with Master UDI-DI

The payload data elements of a UDI with a Master UDI shall include a PPN or HPC to identify the medical device (trade item).

The goal is to reduce the number of data records in EUDAMED using the Master UDI-DI for devices mentioned in the delegated regulation. For this purpose, not the UDI-DI but the Master UDI-DI is to be reported to EUDAMED UDI and Device module.

## 3.4.1. Structure of the Master UDI-DI

The Master UDI-DI is prefixed in the UDI with the Data Identifier “9N” and the PRA Code "MA".

### Master UDI-DI (MUDI)

Diagram showing the structure of the Master UDI-DI: 9N (DI), MA (PRA-Code), 12345 (CIN), MAX19 (Device Group Code), 00 (CC)

Figure 6: Structure of the Master UDI-DI

The Master UDI-DI consists of four elements (substring elements). "MA" is the Product Registration Agency Code of the Master UDI-DI. This is followed by the manufacturer identifier CIN, for which the 5-digit IFA Supplier Number, assigned by IFA, is used. It can be found in the first order confirmation a new customer receives or in the summary of <u>manufacturer address data</u>, which can be requested in IFA portal. The following Device Group Code is the designation of the product group determined by the manufacturer with a maximum of 19-digits. Period (.) can be used for any separation within the Device Group Code. Finally, the two-digit check digit is added, calculated over the complete data field including the "MA" according to Modulo 97. The calculation is described in the IFA document Technical Notes - Check Digit Calculations<sup>10</sup> Technical Notes - Check Digit Calculations.

<sup>10</sup> [https://www.ifaffm.de/mandanten/1/documents/04_ifa_coding_system/IFA-Info_Check_Digit_Calculations_PZN_PPN_UDI_EN.pdf](https://www.ifaffm.de/mandanten/1/documents/04_ifa_coding_system/IFA-Info_Check_Digit_Calculations_PZN_PPN_UDI_EN.pdf)

Version 2.5

<page_number>Page 11 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

The data elements of the Master UDI-DI are specified as follows:

<table>
  <thead>
    <tr>
        <th colspan="6">Master UDI-DI</th>
    </tr>
    <tr>
        <th>Substring element:</th>
        <th>PRA-Code</th>
        <th>CIN</th>
        <th>Device Group Code</th>
        <th colspan="2">Check Digits</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>generated by:</td>
        <td>IFA</td>
        <td>IFA</td>
        <td>Manufacturer</td>
        <td>Modulo 97</td>
        <td></td>
    </tr>
    <tr>
        <td>Data type:</td>
        <td>A</td>
        <td>A/Num<sup>11</sup></td>
        <td>A/Num</td>
        <td>Num</td>
        <td></td>
    </tr>
    <tr>
        <td>Character set:<sup>12</sup></td>
        <td>MA</td>
        <td>0 – 9, A – Z</td>
        <td>0 – 9; A – Z; “.” ;“-”</td>
        <td>0 – 9</td>
        <td></td>
    </tr>
    <tr>
        <td>Character length:</td>
        <td>2</td>
        <td>5</td>
        <td>1 ... 19</td>
        <td>2</td>
        <td></td>
    </tr>
    <tr>
        <td>String length:</td>
        <td> </td>
        <td colspan="3">10 ... 28</td>
        <td> </td>
    </tr>
    <tr>
        <td>Example:</td>
        <td>MA</td>
        <td>12345</td>
        <td>ABCD.12345</td>
        <td>63</td>
        <td></td>
    </tr>
  </tbody>
</table>

Figure 7: Specification of the Master UDI-DI

### 3.4.2. Data string of the Master UDI-DI

In the data string of the Data Matrix, the Master UDI-DI is indicated with the Data Identifier "9N" and PRA-Code "MA". For coding, the ASC data structure (format 06) applies according to subitem A in the IFA specification PPN-Code Specification for Retail Packaging, chapter 5.1. For HPC, alternatively the keyboard compatible syntax DIN 16958 can be used. The following sequence of Master UDI-DI data elements results in the data string:

<9N> Data Identifier PPN
* \<MA> PRA-Code for included Master UDI-DI
* \<CIN> 5-digit IFA Supplier Number
* \<Device Group Code> Designation of the product group
* \<CC> MUDI-Check character: 2-digit Modulo 97

Details of the data structure are described below in chapter 3.6.

Find label examples in chapter 8.

### 3.4.3. HRI format “Interpretation Line”

To represent the data string of the Master UDI-DI in a human readable interpretation line, the format “Interpretation Line” applies. Find details in chapter 4.2.5. below. The data elements of the Master UDI-DI are defined in chapter 3.4.1.

<sup>11</sup> At present, exclusively numeric CIN are assigned.

<sup>12</sup> Corresponding ASCII characters: 48 – 57 for digits 0 – 9; 65 – 90 for characters A – Z; 46 for the “period”.

Version 2.5

Page 12 of 40

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

## 3.5. Basic UDI-DI

The Basic UDI-DI (BUDI) is the main access key for device-related information in EUDAMED. It is referenced in regulatory documents (such as certificates, EU declaration of conformity, technical documentation, summary of safety and (clinical) performance).

With the BASIC-UDI-DI the manufacturer forms groups of products which share the same core characteristics. In accordance with MDCG 2018-1 Guidance on BASIC UDI-DI and changes to UDI-DI these include intended purpose, risk class, essential design and manufacturing characteristics. It is in the responsibility of the manufacturer to lay down the specific details with regards to its products and to document this accordingly. Further information on the allocation of BASIC-UDI-DI and UDI-DI is provided in:

* MDCG 2018-3 Guidance on UDI for systems and procedure packs

* MDCG 2018-5 UDI Assignment to Medical Device Software

* MDCG 2022-7 Questions and Answers on the Unique Device Identification system under Regulation (EU) 2017/745 and Regulation (EU) 2017/746

* MDCG 2020-3 Guidance on significant changes regarding the transitional provision under Article 120 of the MDR with regard to devices covered by certificates according to MDD or AIMDD

Since the Basic UDI-DI does not appear on the package, no data identifier is specified for this data element. For a standardised electronic exchange in XML format, the XML tag “B_UDI_DI” was specified for the Basic UDI-DI.

The Basic UDI-DI is generated from these four elements (substring elements):

### Basic UDI-DI (BUDI)

<table>
    <tr>
        <th>PP</th>
        <th>12345</th>
        <th>MAX16</th>
        <th>12</th>
    </tr>
    <tr>
        <td><mark>IAC</mark></td>
        <td><mark>CIN</mark></td>
        <td><mark>Device Group Code</mark></td>
        <td><mark>CC</mark></td>
    </tr>
</table>

* <mark>IAC</mark> Issuing Agency Code
* <mark>CIN</mark> IFA assigned CIN
* <mark>Device Group Code</mark> Device Group Code
* <mark>CC</mark> Check character

Figure 8: Structure of the BUDI

The Basic UDI-DI consists of four elements (substring elements). "PP" is the Issuing Agency Code IAC for all Basic UDI-DI generated with IFA coding. This is followed by the manufacturer identifier CIN, for which the 5-digit IFA Supplier Number, assigned by IFA, is used. It can be found in the first order confirmation a new customer receives or in the summary of <u>manufacturer address data</u>, which can be requested from <u>IFA portal</u>. The following Device Group Code is the designation of the product group determined by the manufacturer with a maximum of 16-digits. Period (.) can be used for any separation within the Device Group Code. Finally, the two-digit check digit is added, calculated over the complete data field including the "PP" according to Modulo 97. Use the <u>BUDI generator</u> in <u>IFA portal</u> to generate Basic UDI-DI. The calculation is described in the IFA document <u>Technical Notes - Check Digit Calculations</u>.

Version 2.5

<page_number>Page 13 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

<u>[Directory](https://www.ifaffm.de)</u>

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

The data elements of the Basic UDI-DI are specified as follows:

<table>
  <thead>
    <tr>
        <th colspan="6">Basic UDI-DI</th>
    </tr>
    <tr>
        <th>Substring element:</th>
        <th>IAC</th>
        <th>Manufacturer Code<sup>13</sup></th>
        <th>Device Group Code</th>
        <th colspan="2">Check Digits</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>generated by:</td>
        <td>IFA</td>
        <td>IFA</td>
        <td>Manufacturer</td>
        <td>Modulo 97</td>
        <td></td>
    </tr>
    <tr>
        <td>Data type:</td>
        <td>A</td>
        <td>A/Num<sup>14</sup></td>
        <td>A/Num</td>
        <td>Num</td>
        <td></td>
    </tr>
    <tr>
        <td>Character set:<sup>15</sup></td>
        <td>PP</td>
        <td>0 – 9</td>
        <td>0 – 9; A – Z; “.”</td>
        <td>0 – 9</td>
        <td></td>
    </tr>
    <tr>
        <td>Character length:</td>
        <td>2</td>
        <td>5</td>
        <td>1 … 16</td>
        <td>2</td>
        <td></td>
    </tr>
    <tr>
        <td>String length:</td>
        <td> </td>
        <td colspan="3">10 ... 25<sup>16</sup></td>
        <td> </td>
    </tr>
    <tr>
        <td>Example:</td>
        <td>PP</td>
        <td>12345</td>
        <td>ABCD.12345678.90</td>
        <td>04</td>
        <td></td>
    </tr>
  </tbody>
</table>

Figure 9: Specifications of the BUDI

The example elements in the last table row result, without further separators, in the Basic UDI-DI: **“PP12345ABCD.12345678.9004“**.

<sup>13</sup> Designated CIN (Company Identification Number) in the relevant standards.

<sup>14</sup> At present, exclusively numeric manufacturer codes are assigned.

<sup>15</sup> Corresponding ASCII characters: 48 – 57 for digits 0 – 9; 65 – 90 for characters A – Z; 46 for the “period”.

<sup>16</sup> In accordance with Guidance MDCG 2019-1 MDCG Guiding Principles for Issuing Entities Rules on Basic UDI-DI.

Version 2.5

Page 14 of 40

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

<u>Directory</u>
<page_number>

14
</page_number>

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

# 3.6. Data content and requirements for the Data Matrix

The specifications from chapters 5.1 and 5.2 of the IFA specifications PPN-Code Specification for Retail Packaging apply to the structure of the data contents. According to this, the individual data elements are integrated in the data string in the syntax given according to ISO/IEC 15434.

<table>
  <thead>
    <tr>
        <th colspan="9">Data string with example sequence of a PPN + batch</th>
    </tr>
    <tr>
        <th>example</th>
        <th>)&gt;<sup>R</sup><sub>S</sub></th>
        <th>06<sup>G</sup><sub>S</sub></th>
        <th>9N</th>
        <th>111234567842</th>
        <th><sup>G</sup><sub>S</sub></th>
        <th>1T</th>
        <th>A123</th>
        <th><sup>R</sup><sub>S</sub> E<sub>OT</sub></th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td rowspan="2">Data element</td>
        <td>Start sequence Syntax ISO/IEC 15434</td>
        <td>Format identificator ASC DI ISO/IEC 15418</td>
        <td>Data identificator for PPN</td>
        <td>PPN</td>
        <td>Separator</td>
        <td>Data identificator batch</td>
        <td>Batch number</td>
        <td>Stop sequence</td>
    </tr>
  </tbody>
</table>

Figure 10: Structure of the data string

In the form shown above, the data string is converted into a code, whereby the data elements are not separated by spaces.

For the Data Matrix code according to ISO/IEC 16022 a macro can be used, which reduces the start sequence to one command character.

Deviating from the syntax of ISO/IEC 15434, for the HPC the DIN 16598 standard for keyboard compatibility (dot structure) can also be used, see below [chapter 7.2.

# 3.7. Additional data elements

For the UDI (marking with UDI-DI and UDI-PI), all UDI-relevant data elements must be lined up together. If further data elements are included, these must be added after the UDI-PI.

Version 2.5
1 April 2026
Informationsstelle für Arzneispezialitäten – IFA GmbH
Page <page_number>15</page_number> of 40
[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

# 4. Marking with code and plain text format

## 4.1. Coding

For the coding of the UDI (code with UDI-DI and UDI-PI) the symbologies according to ISO/IEC 16022 Data Matrix and ISO/IEC 21471 Data Matrix Rectangular Extension are used.

For symbology, dimensioning of permissible matrix sizes and code sizes including the quiet zones of the Data Matrix, chapters 6.1 to 6.4. of the IFA specification <u>PPN-Code Specification for Retail Packaging</u> apply. The minimum size of a module is specified as 0.25 mm. The manufacturer determines the position of the Data Matrix on the basis of the package layout and the printing conditions according to Section 4.14. Part C Annex VI MDR in such a way that the Data Matrix is accessible during normal operation/storage. The print quality of the Data Matrix is specified in chapter 7 of the IFA Specification <u>PPN-Code Specification for Retail Packaging</u>. According to this, the symbol quality is measured according to ISO/IEC 15415, or in the case of direct marking, according to ISO/IEC 29158, and is outputted in quality grades, with the IFA specification setting grade 1.5 (C) as the minimum requirement. Direct marking (for example laser technology) is subject to specific parameters, see <u>chapter 4.1.1.</u>

## 4.1.1. Direct Marking

The process of direct part marking or direct marking is often referred to as DPM. In this specification, direct marking refers to the application of the code to the device.

In accordance with the MDR, direct marking (DPM) of UDI on medical devices is required for reusable devices intended for refurbishing, e.g. surgical instruments. DPM is applied to metal or plastics by various techniques, such as laser marking machines. Due to the different materials and shapes, such as rounded surfaces, direct marking places increased demands on both marking technology and scanners. The quality of Data Matrix or Data Matrix rectangular codes (DMRE), which are used for direct marking according to the IFA Coding System, is therefore not determined according to ISO/IEC 15415 alone, but together with ISO/IEC 29158 Direct Part Mark (DPM) Quality Guideline.

The MDR allows for the following exceptions to UDI DPM:

* If there are significant space constraints to apply code and HRI, the code is to be given preference according to Section 4.7. Part C Annex VI MDR.

* If DPM is not technically feasible, this requirement is waived according to section 4.10. Part C Annex VI MDR.

In both cases, the reasons given must be demonstrated.

In the development or design phase of a new product, it is mandatory to consider the UDI marking requirement. A label field with suitable properties must be defined in the design.

For coding, the same type of code is used as on labels or other packaging materials with similar characteristics. The options for adapting the codes to small areas may therefore be as follows:

* For square areas, ISO/IEC 16022 Data Matrix is recommended.

* For narrow or rounded areas, the Data Matrix with extended rectangular formats according to ISO/IEC 21471 DMRE is recommended.

Version 2.5

<page_number>Page 16 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[<u>Directory</u>](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

ISO/IEC 16022 defines 6 formats of the Data Matrix code, which are rectangular. If rectangular formats have to be used, it should first be checked whether one of these 6 formats is sufficient. Furthermore, a range of 18 rectangular formats from ISO/IEC 21471 DMRE can be utilized.

For the print quality of DPM, ISO/IEC 15415 standard always applies together with the addition and modification by ISO/IEC 29158.

ISO/IEC 29158 was published in a new version in December 2020. The quality levels have been expanded from 5 to 41.

In contrast to ISO/IEC 15415 standard, which has not yet been updated, the assessment in 41 steps is as follows:

<table>
  <thead>
    <tr>
        <th>ISO grade in 1/10 levels</th>
        <th>ANSI grade</th>
        <th>Meaning</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>3,5; 3,6; 3,7; 3,8; 3,9; 4,0</td>
        <td>A</td>
        <td>Very good</td>
    </tr>
    <tr>
        <td>2,5; 2,6, 2,7, 2,8; 2,9; 3,0; 3,1; 3,2; 3,3; 3,4</td>
        <td>B</td>
        <td>Good</td>
    </tr>
    <tr>
        <td>1,5; 1,6, 1,7, 1,8; 1,9; 2,0; 2,1; 2,2; 2,3; 2,4</td>
        <td>C</td>
        <td>Satisfactory</td>
    </tr>
    <tr>
        <td>0,5; 0,6, 0,7, 0,8; 0,9; 1,0; 1,1; 1,2; 1,3; 1,4</td>
        <td>D</td>
        <td>Adequate</td>
    </tr>
    <tr>
        <td>0,0; 0,1; 0,2; 0,3; 0,4</td>
        <td>F</td>
        <td>Failed</td>
    </tr>
  </tbody>
</table>

*Figure 11: Structure of the data string*

There are still individual criteria that can only assume the state "Passed" (4) or "Not Passed" (0).

Coding by DPM is usually applied on small products with little space for the code or when the product or its intended use excludes the utilisation of labels. Furthermore, due to the material characteristics, the marking surface can be very glossy or allow only a very low absolute contrast. For these reasons, scanners are needed for such markings whose design and reading properties are adapted to these characteristics of the coding. The optical resolution is critical for very small coding. The illumination can also be (very) critical if the code surface is highly reflective.

Version 2.5

<page_number>Page 17 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[<u>Directory</u>](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

Parameters and quality requirements for DPM-Codes:

<table>
  <thead>
    <tr>
        <th>Code type</th>
        <th>Minimal module size</th>
        <th>Maximum module size</th>
        <th>Symbol size</th>
        <th>Light margins</th>
        <th>Minimum quality</th>
        <th>Comments</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>Data Matrix<sup>*5</sup></td>
        <td>0.1 mm<sup>*6</sup></td>
        <td>0.51 mm<sup>*3</sup></td>
        <td>Depending on the data, the module size and format (square or rectangular)</td>
        <td>Min. 1 module on each side, 3 Module recommended<sup>*1</sup></td>
        <td>DPM 1.5<br/>Red light: 660 nm<br/>Synthetic aperture: 80% of measured module width<br/>Illumination Dome (D)<sup><em>2</sup> or Illumination 45°Q<sup></em>4</sup></td>
        <td rowspan="2">Small products, predominantly of metal and predominantly slightly to very glossy surfaces</td>
    </tr>
    <tr>
        <td>DMRE</td>
        <td>0.1 mm</td>
        <td>0.51 mm<sup>*3</sup></td>
        <td>Depending on the data, the module size and format (square or rectangular)</td>
        <td>Min. 1 module on each side, 3 Module recommended<sup>*1</sup></td>
        <td>DPM 1.5<br/>Red light: 660nm<br/>Synthetic aperture: 80% of measured module width<br/>Illumination Dome (D)<sup>*2</sup></td>
    </tr>
  </tbody>
</table>

Figure 12: Requirements for DPM-codes

<sup>\*1</sup> If the light margins are dimensioned to the minimum dimension of one module, the tolerance of the labelling must be taken into account. The light margins must then be larger than the single module whilst matching this tolerance.

<sup>\*2</sup> The scanners used for this purpose must be designated in their technical specification as DPM-capable. For very glossy or reflective substrates these scanners need to be equipped with a very diffuse illumination (e.g. DOME).

<sup>\*3</sup> For slightly larger products, it is advantageous to use larger codes as well. The rule of thumb for the targeted size is: "As large as possible, as small as necessary".

<sup>\*4</sup> 45° Q is a directional illumination from four sides. This illumination is not suitable for reflective surfaces. For matt, very dark materials where the codes have little contrast, this illumination can be used. This is advantageous for scanner selection because the scanner only has to illuminate brightly enough, but there are no special requirements for the illumination.

<sup>\*5</sup> The "short" code sides of Code 128 and Data Matrix rectangular and DMRE have a large distance between them. The wider the code becomes in relation to the height, the more extreme the distance. On high-gloss materials, the illumination (inspection as well as scanner) may no longer be sufficiently diffuse at these edges. The mirror reflections occurring there make it difficult or impossible to read the code.

<sup>\*6</sup> Codes with matrix cell sizes below 0.1mm are technically feasible, but so challenging in terms of marking and reading technology that they should only be used in exceptional cases and in bilateral agreement with healthcare providers.

Version 2.5

Page 18 of 40
<page_number>18</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

<u>Directory</u>

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

All code markings that achieve grade 1.5 or better when using the ISO/IEC 15415 assessment method, may be measured using this method. This is advantageous because it provides evidence that these codes are easily readable with simple and inexpensive scanners. Any code that does not achieve this requirement will be measured as a DPM code according to ISO/IEC 29158. The minimum quality is defined in the above table "Parameters and quality requirements for DPM codes".

Some test equipment manufacturers offer the 45° Q Diffuse illumination variant. This is a primarily directional illumination that is made more homogeneous by diffuser plates or other measures. This arrangement corresponds to a reading application with a directionally illuminating scanner which is used in a bright environment. In this arrangement, the ambient light is very diffuse and mixes with the directional illumination of the scanner. This measurement arrangement can be used to estimate whether codes on critical materials can still be read under the conditions mentioned.

## 4.1.2. Test Requirements of ISO/IEC 29158

The test criteria and principles of ISO/IEC 15415 apply, and thus the IFA Specification <u>PPN-Code for Retail packaging</u>, which briefly explains them.

Amendments result as follows:

*   **Exposure**
    According to ISO/IEC 15415, the illumination is set to a fixed value so that the reflectance values as a measured value are related to national standards (PTB, NIST). In the DPM testing, the illumination is changed until a high-contrast image is obtained which, if possible, corresponds in contrast impression to that of a code on a white paper. Depending on the absolute contrast, the exposure setting changes.

*   **Minimum reflection (Rtarget)**
    This parameter is additionally introduced in ISO/IEC 28158. It specifies a relation to a contrast value of a calibration card. A calibration card provides measured values that can be referenced in a traceable way to national standards. The larger the value for Rtarget becomes, the closer the exposure setting is to the fixed value for the ISO/IEC 15415 assessment. The smaller the value becomes, the stronger the illumination must be. The measured value is classified in 41 levels. In practical use, it is possible to find products with high and very low contrast. Scanners with fixed illumination settings may only be able to read the light codes and not the very dark ones, or vice versa. In that case, a range that can be used for Rtarget can be defined in addition to the minimum quality. The definition depends on the characteristics of the scanners used.

*   **Cell contrast**
    The cell contrast replaces the symbol contrast of ISO/IEC 15415. The calculation is based on the determined reflectance values, which result adaptively according to the above description for exposure. Furthermore, unlike the symbol contrast, the cell contrast is calculated from (light - dark) / light. Thus, the assessment is more sensitive to brightening dark areas than to the simple contrast light - dark.

*   **Cell modulation**
    Cell modulation replaces the reflectance margin and modulation parameters of ISO/IEC 15415. The assessment algorithm is the same. The reflection values in cell modulation are those determined by the adaptive exposure setting of ISO/IEC 29158.

Version 2.5

Page 19 of 40
<page_number>19</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[<u>Directory</u>](https://www.ifagmbh.de)

IFA Coding System for medical devices IFA CODINGSYSTEM logo

The remaining parameters GNU, ANU, FPD, decoding are determined in the same way as in ISO/IEC 15415. The different exposure and reflection values according to ISO/IEC 29158 also affect the assessment of these parameters.

For all parameters, even if this is not yet specified in ISO/IEC 15415, the assessment is in classes or grades in steps of 1/10 instead of 1. ISO/IEC 29158 has adopted this finer classification. This applies until the symbology standards and ISO/IEC 15415 introduce it.

## 4.1.3. Marking of packaging levels

Each packaging level must be marked with its own UDI. Manufacturers draw, under their own responsibility, distinctions between higher packaging levels and shipping containers, the latter of which do not have to be marked with a UDI according to section 3.2 Part C Annex VI MDR.

The parameters and measurement methods for marking packaging levels differ from those for DPM.

The following table lists the different packaging levels, the available code types and their requirements.

<table>
  <thead>
    <tr>
        <th>Packaging level</th>
        <th>Code type</th>
        <th>Requirement Quality / Code size</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>Device</td>
        <td>Data Matrix and DMRE</td>
        <td>Chapter 4.1.1.</td>
    </tr>
    <tr>
        <td>Retail pack / Secondary packaging</td>
        <td>Data Matrix and DMRE</td>
        <td>IFA Specification PPN-Code for Retail packaging, chapter 6</td>
    </tr>
    <tr>
        <td>Shipping container</td>
        <td>Data Matrix, DMRE, Code 128, Specific freight forwarder code</td>
        <td>IFA Transportation Logistics Specification<sup>17</sup>, requirements of the freight forwarder</td>
    </tr>
  </tbody>
</table>

Figure 13: Options for packaging levels

For secondary packaging, the requirements of the IFA Specification PPN-Code for Retail packaging chapter 6 and chapter 7 apply. As a difference to medicinal products, it should be noted that other data are required depending on the risk class (e.g. without serial number but with manufacturing date and without expiry date).

For shipping containers, the requirements of the IFA Transport Logistics Specification apply. It should be noted that almost every carrier/freight forwarder has its own specification regarding the transport label. These must be observed and have priority.

For details on labelling requirements of drug-device combinations see the EMA Questions & Answers for applicants, marketing authorisation holders of medicinal products and notified bodies with respect to the implementation of the Medical Devices and In Vitro Diagnostic Medical Devices Regulations ((EU) 2017/745 and (EU) 2017/746) (EMA/37991/2019).

<sup>17</sup> [https://www.ifaffm.de/mandanten/1/documents/04_ifa_coding_system/IFA_Spec_Transport_Logistik_EN.pdf](https://www.ifaffm.de/mandanten/1/documents/04_ifa_coding_system/IFA_Spec_Transport_Logistik_EN.pdf)

Version 2.5 <page_number>Page 20 of 40</page_number>

1 April 2026 Informationsstelle für Arzneispezialitäten – IFA GmbH [Directory](Directory)

IFA Coding System for medical devices IFA CODINGSYSTEM logo

# 4.1.4. Parameters and quality requirements for medical device packaging

The specifications from chapter 6 of the IFA Specification PPN-Code for Retail packaging are summarized here for application to medical device packaging:

<table>
  <thead>
    <tr>
        <th>Code type</th>
        <th>Minimal module size</th>
        <th>Maximum module size</th>
        <th>Symbol size</th>
        <th>Light margins</th>
        <th>Minimum quality</th>
        <th>Comments</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>Data Matrix</td>
        <td rowspan="2">0.25mm</td>
        <td rowspan="2">0.99mm</td>
        <td rowspan="2">Depending on the data, the module size and format (square or rectangular)</td>
        <td rowspan="2">Min. 1 module on each side, 3 modules recommended</td>
        <td rowspan="2">DPM 1.5<br/>Red light: 660nm<br/>Synthetic aperture: 80% of measured module width<br/>Illumination 45° Q</td>
        <td>Code testing according to ISO/IEC 15415 is permitted (see end of chapter 4.1.1. for explanation).</td>
    </tr>
    <tr>
        <td>DMRE</td>
        <td>Sales packaging whose codes are read at the checkout in the pharmacy should always be checked according to ISO/IEC 15415 if possible.</td>
    </tr>
  </tbody>
</table>

*Figure 14: Requirements medical device packagingl*

# 4.2. UDI-Labelling in plain text format

## 4.2.1. General information

The MDR requires to provide the product with the UDI in machine-readable format (AIDC format) and in human readable format (HRI format). Section 4. Part C Annex VI MDR describes exceptions to this principle.

All elements of the UDI have to appear in the HRI format. In accordance with Section 4.8 Part C Appendix VI MDR, IFA determines the HRI formats in this specification. IFA takes into account the manufacturer’s varying needs in terms of their products, markets and established labelling.

In the following chapters, IFA specifies three formats from which the manufacturer can choose according to his circumstances. The manufacturer must take all aspects into account, including those to be considered in addition to the MDR.

To ensure readability, the explications of the so-called EU Readability Guideline must be followed (Guideline on the readability of the labelling and package leaflet of medicinal products for human use) .

Version 2.5 <page_number>Page 21 of 40</page_number>
1 April 2026 Informationsstelle für Arzneispezialitäten – IFA GmbH [Directory](https://www.ifaffm.de/en/ifa-coding-system/ifa-specification-ppn-code-for-retail-packaging.html)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

## 4.2.2. Marking with PZN

Medical devices made available in German pharmacies which are reimbursed by the statutory health insurance must be labelled with the PZN in accordance with the framework agreement pursuant to Section 131 of the German Social Code, Book V (SGB V). Products made available in this market must be labelled with a machine-readable PZN and a plain text line. For other products, this is required if the PZN is needed for logistical purposes or for reimbursement.

The PZN can be coded either as a PPN in the Data matrix or separately in Code 39. If the PZN is coded as a PPN in the Data Matrix, the short identifier "PZN: " is preceding the plain text line. If the PZN is additionally represented in Code 39, the short identifier "PZN -" is preceding the plain text line.

## 4.2.3. HRI format “Symbol”

In this format, symbols or abbreviations commonly used at international level precede the corresponding UDI data as HRI qualifiers (identifiers). The UDI-DI is highlighted by means of a short identifier.

For the elements of the UDI the following applies:

*   **UDI-DI:** The UDI-DI is to be affixed with a short identifier: “UDI-DI (PPN): “. The term in brackets indicates that a PPN is being used.

**Example: UDI-DI (PPN): 111234567842**

*   **UDI-PI:** All elements of the UDI-PI must appear in the HRI format. UDI-PI data must be prefixed with symbols or alternatively the short identifiers resulting from the legal regulations or from the manufacturer’s QM system. The layout is to be configured in a way the user can match the data inevitably. Short identifier consisting of data strings must be separated from data with a colon and spaces.

For usage of date specifications YYYY-MM-DD or YYYY-MM must be used unless legal provisions or the manufacturer’s QM system ask for a different format.

**Example:**

Example of a medical device label with Data Matrix code and HRI symbols

For more examples see chapter 6.

Figure 15: Example HRI format “Symbol”

Version 2.5

<page_number>Page 22 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

## 4.2.4. HRI format “Symbol +”

This format is based on the "Symbol" format described in the previous chapter. The only difference is that the identifier used in the code is added to the symbols.

This format is particularly suitable for the exceptions in which, according to the legislation, the UDI in AIDC format can be omitted and where the UDI is placed on the label in HRI format only. However, in such cases, the PZN must be applied as a code and in plain text if the product falls within the scope of the framework agreement pursuant to section 131 SGB V.

Examples:

Example label with PZN in Code 39 barcode

Example label with PZN (PPN) in Data Matrix code

*Figure 16: Example with PZN in Code 39*

*Figure 17: Example with PZN (PPN) in Data Matrix*

The following table shows the identifiers to be assigned to the corresponding data elements in the HRI format:

<table>
  <thead>
    <tr>
        <th>UDI Element</th>
        <th>Data element<sup>18</sup></th>
        <th>Data identifying label<sup>19</sup></th>
        <th>Examples</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>UDI-DI</td>
        <td>&lt;PPN&gt;</td>
        <td>UDI-DI (9N):</td>
        <td>111234567842</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>&lt;LOT&gt;</td>
        <td>(1T):</td>
        <td>1234AB</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>&lt;EXP&gt;</td>
        <td>(D):</td>
        <td>2024-10-31</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>&lt;MFD&gt;</td>
        <td>(16D):</td>
        <td>2019-08-31</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>&lt;SN&gt;</td>
        <td>(S):</td>
        <td>12345678AB</td>
    </tr>
  </tbody>
</table>

*Figure 18: Identifier for the HRI format “Symbol +”*

For plain text representation, as many line breaks as necessary may be used, as long as interpretation is conclusive for the user.

<sup>18</sup> Listed accordingly to the XML node.

<sup>19</sup> Attention must be drawn to the colon “: “ after the closing bracket of the short identifier.

Version 2.5
<page_number>Page 23 of 40</page_number>
1 April 2026
Informationsstelle für Arzneispezialitäten – IFA GmbH
[Directory](Directory)

IFA Coding System for medical devices IFA CODINGSYSTEM logo

# 4.2.5. HRI format “Interpretation Line”

The HRI format "interpretation line" provides for the output of the data fields with the corresponding data identifiers in exactly the same way as they are represented in the code. To delimit the data fields, the data identifiers used are to be placed in round brackets "()".

In addition to the interpretation line, the other information on the label resulting from the relevant legal regulations and the manufacturer's QM system must be added together with qualifiers commonly used at international level.

This format conforms with the representations in the IMDRF UDI System Application Guide<sup>20</sup> and is suitable for cases in which the manufacturer has to consider these formats as well.

Example:

Example label showing MD, Device Name, PZN: 12345678, LOT ABC12345, Data Matrix code, expiration date 2024-12-31, HRI string (9N)111234567842(1T)ABC12345(D)241231, Company Name, and CE mark.

Figure 19: Example HRI format “Interpretation Line”

The data encoded in the Data Matrix with syntax ISO/IEC 15434 result in the following data string, which consists of the control characters and payload data and is automatically coded by the system software for marking:

`)><RS>06<GS>9N111234567842<GS>1TABC12345<GS>D241231<RS><EOT>`

The associated HRI does not show control characters because they are not printable, but it does show the ASC data identifiers in brackets:

(9N)111234567842(1T)ABC12345(D)241231

Furthermore, the interpretation line, like the "Symbol +" format described above, is suitable for the special constellations in which the MDR allows to omit the AIDC format of the UDI.

Requirements for machine readability and representation of the PZN in plain text format see [chapter 4.2.2.

<sup>20</sup> International Medical Device Regulators Forum Unique Device Identification system Application Guide , aka IMDRF N48 guidance document

Version 2.5 <page_number>Page 24 of 40</page_number>
1 April 2026 Informationsstelle für Arzneispezialitäten – IFA GmbH [Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

## 4.2.6. Peculiarities

If there are significant constraints limiting the use of both formats – AIDC and HRI – on the label, Section 4.7 Part C Appendix VI MDR allows to omit the HRI format and grants to apply the AIDC format only. But for products being generally used outside of healthcare facilities the HRI format is to be used primarily even if this results in there being no space for the AIDC format.

If the UDI is represented exclusively in HRI format, the format “Symbol +” according to chapter 4.2.4. or “interpretation line” according to chapter 4.2.5. must be applied. Examples see Figure 16, 17 or 19.

With the exceptions described in the MDR, when the AIDC or HRI format of the UDI can be omitted, the PZN must be applied as a code and plain text if the product is subject to the framework agreement pursuant to Section 131 of the German Social Code, Book V (SGB V). (see <u>chapter 4.2.2.</u>).

## 4.3. HRI format for documentation and records

In addition to the HRI representation on the packages, there is the necessity to provide the UDI in documents.

For the correct interpretation of the data fields and contents, two formats are defined for the representation:

* Output in XML format or

* Output in format of the Data Identifiers

The XML format is preferred. It offers the advantage of universal representation and further processing, is detached from the specific machine language used in the code and is therefore commonly understood.

Version 2.5

<page_number>Page 25 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[<u>Directory</u>](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

### 4.3.1. XML format

When choosing the XML format, XML nodes as defined in Appendix A and other commonly used data field identifiers shall be prefixed. The data contents are to be outputted as stored in the code so that the data formats according to Appendix A must be observed for date specifications.

The data string is structured according to XML standards. Thus, hierarchical representation is also possible.

**Example 1** – XML format (without hierarchy):

`<PPN>111234567842<LOT>A1234<MFD>20200826`

**Example 2** – XML format in hierarchical structure:

```xml
<UDI>
    <UDI_DI>
        <PPN>111234567842
    </UDI_DI>
    <UDI_PI>
        <LOT>A1234
        <MFD>20200826
    </UDI_PI>
</UDI>
```

### 4.3.2. Data Identifier Format

In this format, the Data Identifiers and data contents are represented as they are contained in the code. For distinction the data identifiers are indicated in round brackets. The data contents are identical to the XML format (see above).

**Example:**

(9N)111234567842(1T)A1234(16D)20200826

### 4.4. Emblem for the Data Matrix Code

If the space and printing techniques allow, it is recommended to affix the “UDI: ” emblem as a reference to the UDI carrier near the Data Matrix. In doing so, the spacing (quiet zone) to the code must be observed.

Version 2.5
Page 26 of 40
1 April 2026
Informationsstelle für Arzneispezialitäten – IFA GmbH
[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

# 5. EUDAMED

EUDAMED is the central UDI database run by the Commisson. EUDAMED is structured in 6 modules and a public website<sup>21</sup>:

* Actors registration

* UDI/Devices registration

* Notified Bodies and Certificates

* Clinical Investigation and performance studies

* Vigilance and post-market surveillance

* Market Surveillance

Manufacturers can register in EUDAMED and apply for SRN (Single Registration Number). They may also register devices on a voluntary basis. It should be noted that even products that are not subject to UDI as so-called legacy devices may become subject to registration. Details can be found in the Commission's document "Management of Legacy Devices MDR EUDAMED".

Note: IFA does not transfer any data to EUDAMED. Therefore, the publication of products via the IFA information services does not exempt manufacturers from reporting to EUDAMED.

Finally, reference should be made to the EUDAMED EUD Data Dictionary. For the assignment of UDI-DI, the data fields in the product-related spreadsheets that cannot be changed ("Updateable" is empty) must be taken into account. Changes to these data fields require the assignment of new UDI-DI.

<sup>21</sup> [https://ec.europa.eu/tools/eudamed/#/screen/home](https://ec.europa.eu/tools/eudamed/#/screen/home)

Version 2.5

Page 27 of 40

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

# 6. Examples of UDI marking on medical devices

The following examples show different variants for marking UDI carrier. The tables represent the data fields with the data identifiers for coding in the Data Matrix. The associated labels bear the code, the HRI and other exemplary plain text.

## 6.1. Example 1 – Medical device without separate PZN

Label example for Medical device without separate PZN showing Device Name, PPN, LOT, expiry date, Data Matrix code, and company information

<table>
  <thead>
    <tr>
        <th colspan="3">AIDC</th>
    </tr>
    <tr>
        <th>UDI</th>
        <th>DI</th>
        <th>Payload data</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>UDI-DI</td>
        <td>9N</td>
        <td>111234567842</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>1T</td>
        <td>ABC12345</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>D</td>
        <td>241231</td>
    </tr>
  </tbody>
</table>

**Comments:**

Example with PPN, batch and encoded expiry date, without separate labelled PZN. In the PPN, a PZN is included.

Version 2.5

Page 28 of 40
<page_number>28</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[<u>Directory</u>](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

## 6.2. Example 2 – Medical device software for download

Screenshot of "About MDSW Name" window showing product name, LOT, UDI, CE mark, manufacturer details, and date of manufacture

<table>
  <thead>
    <tr>
        <th colspan="3">HRI</th>
    </tr>
    <tr>
        <th>UDI</th>
        <th>DI</th>
        <th>Payload data</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>UDI-DI</td>
        <td>9N</td>
        <td>1312345170400033</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td> </td>
        <td>170400XYZ</td>
    </tr>
  </tbody>
</table>

**Comments:**

Example of an electronic display with HPC. In the HPC a zero is included as packaging level index. Since no AIDC is present, only Data Identifier 9N precedes the UDI-DI.

See MDCG 2025-4 Guidance on the safe making available of medical device software (MDSW) apps on online platforms.

Version 2.5

Page 29 of 40
<page_number>29</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

<u>[Directory](Directory)</u>

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

## 6.3. Example 3 – Medical device software download + DVD

Illustration of a medical device software DVD and its label containing UDI, PPN, LOT, and manufacturer information

<table>
  <thead>
    <tr>
        <th colspan="3">AIDC</th>
    </tr>
    <tr>
        <th>UDI</th>
        <th>DI</th>
        <th>Payload data</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>UDI-DI</td>
        <td>9N</td>
        <td>111234567842</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>1T</td>
        <td>170400XYZ</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>16D</td>
        <td>20210417</td>
    </tr>
  </tbody>
</table>

**About MDSW Name** [x]

**MDSW Name**

**LOT**: 170400XYZ
**UDI**: (9N)111234567842
(1T)170400XYZ
(16D)20210417

**CE Medical Device**

**Copyright 2021 MDSW AG**

**MANUFACTURER**: MDSW AG
**ADDRESS**: Hauptstr. 1
12345 Citystadt
Germany

**DATE OF MANUFACTURE**: 2021-04-17

### Comments:

Example of a MDSW for download and on DVD. A PPN is used for UDI-DI.

Data Identifier of the version number/lot is 1T. The manufacturing date is also encoded as UDI-PI. For more on this refer to the guidance document MDCG 2021-10 - The status of Appendixes E-I of IMDRF N48 under the EU regulatory framework for medical devices.

Version 2.5

<page_number>Page 30 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[<u>Directory</u>](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

## 6.4. Example 4 – Batch-related medical device

Label example for batch-related medical device with PPN, batch, expiry date, and PZN in plain text

<table>
  <thead>
    <tr>
        <th colspan="3">AIDC</th>
    </tr>
    <tr>
        <th>UDI</th>
        <th>DI</th>
        <th>Payload data</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>UDI-DI</td>
        <td>9N</td>
        <td>111234567842</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>1T</td>
        <td>ABC12345</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>D</td>
        <td>241231</td>
    </tr>
  </tbody>
</table>

**Comments:**

Example with PPN, batch and encoded expiry date, PZN in plain text.

## 6.5. Example 5 – Medical device with UDI and PZN in Code 39

Label example for medical device with UDI and PZN in Code 39

<table>
  <thead>
    <tr>
        <th colspan="3">UDI AIDC</th>
    </tr>
    <tr>
        <th>UDI</th>
        <th>DI</th>
        <th>Payload data</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>UDI-DI</td>
        <td>9N</td>
        <td>111234567842</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>1T</td>
        <td>ABC12345</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>D</td>
        <td>241231</td>
    </tr>
  </tbody>
</table>

**Comments:**

Example with PPN, batch and encoded expiry date, with PZN in code 39.

Version 2.5

<page_number>Page 31 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

## 6.6. Example 6 – Medical device with URL in the Data Matrix

Medical device label with URL in Data Matrix

<table>
  <thead>
    <tr>
        <th colspan="3">AIDC</th>
    </tr>
    <tr>
        <th>UDI</th>
        <th>DI</th>
        <th>Payload data</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>UDI-DI</td>
        <td>9N</td>
        <td>111234567842</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>D</td>
        <td>240600</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>33L</td>
        <td>http://Produkthinweis.de</td>
    </tr>
  </tbody>
</table>

Comments:

Following the UDI a URL is included in the Data Matrix.

## 6.7. Example 7 – Serialized medical device

Serialized medical device label

<table>
  <thead>
    <tr>
        <th colspan="3">AIDC</th>
    </tr>
    <tr>
        <th>UDI</th>
        <th>DI</th>
        <th>Payload data</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>UDI-DI</td>
        <td>9N</td>
        <td>111234567842</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>S</td>
        <td>JXCC263D0889</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>1T</td>
        <td>170400XYZ</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>D</td>
        <td>230617</td>
    </tr>
  </tbody>
</table>

Comments:

Serialised medical device with German PZN „12345678“ in plain text.

Version 2.5

Page 32 of 40
<page_number>32</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

# 7. Examples for UDI marking with HPC

The following examples show product labels with UDI in form of HPC encoded in syntax ISO/IEC 15434 and alternatively in syntax DIN 16598.

## 7.1. Example 8 – Medical device with HPC

Medical device label example with UDI Data Matrix code

<table>
  <thead>
    <tr>
        <th colspan="3">AIDC</th>
    </tr>
    <tr>
        <th>UDI</th>
        <th>DI</th>
        <th>Payload data</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>UDI-DI</td>
        <td>9N</td>
        <td>1312345MED777094</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>1T</td>
        <td>ABC12345</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>D</td>
        <td>241231</td>
    </tr>
  </tbody>
</table>

Comments:

Medical device with item/product reference lowest packaging level (Unit of Use, Index "0").
Data Identifier for HPC is „9N“.
HPC example generated with data elements:
- PRA-Code „13“ for HPC of the manufacturer
- 5 digit IFA-Supplier number of the manufacturer „12345“
- item/product reference of the manufcturer „MED777“
- Packaging level index Unit of Use, „0“
- PPN-check digits „94“

The data encoded in Data Matrix with syntax ISO/IEC 15434 result in the following data string, which consists of control and payload data and is automatically coded by the system software for marking:

`[)><RS>06<GS>9N1312345MED777094<GS>1TABC12345<GS>D241231<RS><EOT>`

The respective HRI does not display any control characters because these are not printable. The ASC Data Identifiers prefix the UDI data in brackets:

(9N)1312345MED777094(1T)ABC12345(D)241231

Version 2.5
1 April 2026
Informationsstelle für Arzneispezialitäten – IFA GmbH
Page 33 of 40
[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

## 7.2. Example 9 – Medical device with HPC coding DIN 16598

Medical device label example showing MD icon, Device Name, LOT ABC12345, expiration date 2024-12-31, PPN Data Matrix code, HRI string (9N)1312345MED777094(1T)ABC12345(D)241231, manufacturer logo, Company Name, City - Street, Country, website, and CE mark

<table>
  <thead>
    <tr>
        <th colspan="3">AIDC</th>
    </tr>
    <tr>
        <th>UDI</th>
        <th>DI</th>
        <th>Payload data</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>UDI-DI</td>
        <td>9N</td>
        <td>1312345MED777094</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>1T</td>
        <td>ABC12345</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>D</td>
        <td>241231</td>
    </tr>
  </tbody>
</table>

**Comments:**

In this example, the Data Identifiers according to DIN°16598 are used in the Data Matrix in the syntax for keyboard and internet compatible coding. The PPN-emblem above the Data Matrix is optional.

The data coded in the Data Matrix with syntax DIN 16598 result in the following keyboard compatible data string consisting of the control characters "dot" and "roof" and the payload data:

> .9N1312345MED777094^1TABC12345^D241231

The separator „^“ and the leading dot “.” are not displayed in the HRI, but the ASC-DI are bracketed as well as in example 1:

> (9N)1312345MED777094(1T)ABC12345(D)241231

Version 2.5

<page_number>Page 34 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[<u>Directory</u>](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

## 7.3. Example 10 – HPC with various packaging levels


  
    Illustration of multiple retail units with UDI labels
  
  
    

    <table style="width:100%">
      <thead>
        <tr>
          <th colspan="3" style="background-color: #004a99; color: white; text-align: center;">AIDC<br/>(device level)</th>
        </tr>
        <tr>
          <th>UDI</th>
          <th>DI</th>
          <th>Payload data</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td style="background-color: #fff2cc;">UDI-DI</td>
          <td style="background-color: #fff2cc;">9N</td>
          <td style="background-color: #fff2cc;">1312345MED777094</td>
        </tr>
        <tr>
          <td style="background-color: #dae8fc;">UDI-PI</td>
          <td style="background-color: #dae8fc;">1T</td>
          <td style="background-color: #dae8fc;">ABC12345</td>
        </tr>
        <tr>
          <td style="background-color: #dae8fc;">UDI-PI</td>
          <td style="background-color: #dae8fc;">D</td>
          <td style="background-color: #dae8fc;">241231</td>
        </tr>
      </tbody>
    </table>
    
      UDI-DI lowest level with index „0“
      HPC example with data elements:
      PRA-Code „13“ for HPC
      5 digit IFA-Supplier Number „12345“
      Item/Ref „MED777“
      Index lowest level (Unit of Use) „0“
      PPN-Check digit „94“
      UDI-PI same as retail pack.
    
  



  
    Illustration of a higher packaging level box with UDI label
  
  
    

    <table style="width:100%">
      <thead>
        <tr>
          <th colspan="3" style="background-color: #004a99; color: white; text-align: center;">AIDC<br/>(Higher packaging level)</th>
        </tr>
        <tr>
          <th>UDI</th>
          <th>DI</th>
          <th>Payload data</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td style="background-color: #fff2cc;">UDI-DI</td>
          <td style="background-color: #fff2cc;">9N</td>
          <td style="background-color: #fff2cc;">1312345MED777227</td>
        </tr>
        <tr>
          <td style="background-color: #dae8fc;">UDI-PI</td>
          <td style="background-color: #dae8fc;">1T</td>
          <td style="background-color: #dae8fc;">ABC12345</td>
        </tr>
        <tr>
          <td style="background-color: #dae8fc;">UDI-PI</td>
          <td style="background-color: #dae8fc;">D</td>
          <td style="background-color: #dae8fc;">241231</td>
        </tr>
      </tbody>
    </table>
    
      HPC example with data elements:
      PRA-Code „13“ for HPC,
      5 digit IFA-Supplier Number „12345“,
      Item/Ref. Manufacturer „MED777“,
      Packaging level Index for pack of 3 „2“,
      PPN-Check digit „27“
    
  


Comments:

Example 10 with index "2" on the higher packaging level of the retail pack and index "0" on the lowest packaging level containing the retail unit. The PPN-emblem above the Data Matrix is optional.

UDI of the higher retail package differs by index "2" from the lowest level with index "0", this way two different UDI-DI are generated.

Manufacturers can equip different package sizes of retail units either with a different item reference for UDI-DI or the item reference remains the same and is marked with a different packaging level index with a number from 0 to 8. In both cases, this index precedes the check digits as the last digit before the check digits of the HPC.

Version 2.5

<page_number>Page 35 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

The lowest level, e.g. the DPM Unit of Use or the blister pack, could be typically provided with the index "0". In this case, for example, a "1" results for the single packaging, and a "2" for the next multiple packaging, whereby index values from 0 to 8 are suitable for UDI.

A distinction must be drawn between retail units that are subject to UDI and shipping containers used for transport. Such shipping containers or logistics units are not subject to UDI. However, it is still advisable to apply an AIDC marking for logistics, which is given the index of a higher packaging level. This marking is not to be marked as UDI and is not to be registered in EUDAMED.

Version 2.5

Page 36 of 40
<page_number>36</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

<u>[Directory](Directory)</u>

IFA Coding System for medical devices IFA CODINGSYSTEM logo

# 8. Examples for UDI marking with Master UDI-DI

The following examples show product labels with UDI-DI and Master UDI-DI.

## 8.1. Example 11 – Medical device with PPN and Master UDI-DI

<table>
  <thead>
    <tr>
        <th colspan="3">AIDC</th>
    </tr>
    <tr>
        <th>UDI</th>
        <th>DI</th>
        <th>Payload data</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>UDI-DI</td>
        <td>9N</td>
        <td>111234567842</td>
    </tr>
    <tr>
        <td>Master UDI-DI</td>
        <td>9N</td>
        <td>MA12345MAX1900</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>1T</td>
        <td>A123</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>16D</td>
        <td>20251120</td>
    </tr>
  </tbody>
</table>

Product label for Example 11 showing Device Name, PZN: 12345678, UDI-DI (PPN): 111234567842, MUDI: MA12345MAX1900, LOT A123, manufacturing date 2025-11-20, Data Matrix code, and CE mark.

**Comments:**

Example with PPN, Master UDI-DI, batch and encoded manufacturing date, without separate labelled PZN. In the PPN, a PZN is included.

## 8.2. Example 12 – Medical device with HPC and Master UDI-DI

Example UDI-DI / HPC:

<table>
  <thead>
    <tr>
        <th colspan="3">AIDC</th>
    </tr>
    <tr>
        <th>UDI</th>
        <th>DI</th>
        <th>Payload data</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>UDI-DI</td>
        <td>9N</td>
        <td>1312345MAX18076</td>
    </tr>
    <tr>
        <td>Master UDI-DI</td>
        <td>9N</td>
        <td>MA12345MAX1900</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>1T</td>
        <td>A123</td>
    </tr>
    <tr>
        <td>UDI-PI</td>
        <td>16D</td>
        <td>20251120</td>
    </tr>
  </tbody>
</table>

Product label for Example 12 showing Device Name, UDI-DI (HPC): 1312345MAX18076, MUDI: MA12345MAX1900, LOT A123, manufacturing date 2025-11-20, Data Matrix code, and CE mark.

**Comments:**

Example with HPC, Master UDI-DI, batch and encoded manufacturing date, without separate labelled PZN. In the PPN, a PZN is included.

Version 2.5 <page_number>Page 37 of 40</page_number>
1 April 2026 Informationsstelle für Arzneispezialitäten – IFA GmbH [Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

# 9. Appendix A: Overview and reference of the data identifiers

The table below specifies the characteristics of the individual data identifiers including the assigned XML nodes:

<table>
  <thead>
    <tr>
        <th></th>
        <th>Data elements</th>
        <th>XML nodes</th>
        <th>DI</th>
        <th>Data -type</th>
        <th>Data format</th>
        <th>Character length</th>
        <th>Character set</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td rowspan="2">UDI-DI</td>
        <td>Pharmacy<br/>Product<br/>Number (PPN)</td>
        <td>&lt;PPN&gt;</td>
        <td>9N</td>
        <td>AN</td>
        <td>—</td>
        <td>11 – 28</td>
        <td>0 – 9; A – Z<br/>no special characters, no use of lowercase letters, no national characters</td>
    </tr>
    <tr>
        <td>Master UDI-DI</td>
        <td>&lt;MA&gt;</td>
        <td>9N</td>
        <td>AN</td>
        <td>&lt;MA&gt;+<br/>&lt;CIN&gt;+<br/>&lt;Device Group Code&gt;+<br/>&lt;CC&gt;</td>
        <td>10 – 28</td>
        <td>Numeric or alphanumeric characters, no national characters</td>
    </tr>
    <tr>
        <td rowspan="4">UDI-PI</td>
        <td>Batch number</td>
        <td>&lt;LOT&gt;</td>
        <td>1T</td>
        <td>AN</td>
        <td>—</td>
        <td>1 – 20</td>
        <td>Numeric or alphanumeric characters, no national characters</td>
    </tr>
    <tr>
        <td>Expiry date</td>
        <td>&lt;EXP&gt;</td>
        <td>D</td>
        <td>Date</td>
        <td>YYMMDD</td>
        <td>6</td>
        <td>0 – 9</td>
    </tr>
    <tr>
        <td>Manufacturing Date</td>
        <td>&lt;MFD&gt;</td>
        <td>16D</td>
        <td>N</td>
        <td>YYYYMMDD</td>
        <td>8</td>
        <td>0 – 9</td>
    </tr>
    <tr>
        <td>Serial number</td>
        <td>&lt;SN&gt;</td>
        <td>S</td>
        <td>AN</td>
        <td>—</td>
        <td>1 – 20</td>
        <td>Numeric or alphanumeric characters, no national characters</td>
    </tr>
    <tr>
        <td rowspan="4">Other elements</td>
        <td>Quantity</td>
        <td>&lt;QTY&gt;</td>
        <td>Q</td>
        <td>N</td>
        <td>—</td>
        <td>1 – 8</td>
        <td>0 – 9</td>
    </tr>
    <tr>
        <td>Price</td>
        <td>&lt;PRICE&gt;</td>
        <td>27Q</td>
        <td>AN</td>
        <td>0.00</td>
        <td>1 – 20</td>
        <td>0 – 9;<br/>“.” as decimal point</td>
    </tr>
    <tr>
        <td>Hyperlink</td>
        <td>&lt;URL&gt;</td>
        <td>33L</td>
        <td>AN</td>
        <td>—</td>
        <td> </td>
        <td> </td>
    </tr>
    <tr>
        <td>National Trade Item Number (NTIN)</td>
        <td>&lt;GTIN&gt;</td>
        <td>8P</td>
        <td>N</td>
        <td>—</td>
        <td>14</td>
        <td>0 – 9</td>
    </tr>
  </tbody>
</table>

Figure 20: Data identifiers

Version 2.5 <page_number>Page 38 of 40</page_number>
1 April 2026 Informationsstelle für Arzneispezialitäten – IFA GmbH [Directory](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

<u>Note:</u>

Details for the data elements can be found in the IFA specifications PPN-Code Specification for Retail Packaging. It describes e.g. the applied character lengths and the format specifics of the expiry date.

**Recommendations for the character set for serial number and batch number:**

a) The character string should only include either uppercase or lowercase letters of the Latin alphabet.

b) To avoid human reading errors and depending on the font used and print quality, similar characters that are prone to be mistaken for each other should not be used. These include e.g.: i, j, l, o, q, u and I, J, L, O, Q, U.

c) While some special characters are technically processed<sup>22</sup>, they should not be used because the risk of misinterpretation is very high. A misinterpreted code results in a package being unable to be identified.

d) If delimiters are necessary within a batch number, the use of a hyphen “-” or underscore “_” or period “.”<sup>23</sup> is ecommended.

<sup>22</sup> The special characters with the decimal ASCII code values of 35 (#), 36 ($), 64 (@), 91 ([), 92 (\\), 93 (]), 94 (^), 96 (`), 123 ({), 124(|), 125 (}), 126 (~) and 127 (¦) and all control characters (ASCII code value 00 – 31) are excluded from technical processing. In principle, all ASCII characters with a decimal value of more than 127 are excluded. The technically allowable characters are in accordance with “GS1 AI encodable character set 82” (GS1 General Specifications, section 7.11 (Figure 7/11-1)).

<sup>23</sup> The use of the period character is particularly recommended, since its location is identical in German and English keyboards. If the wrong language is selected for the keyboard scanners used, the risk of misinterpretation therefore does not exist per se.

Version 2.5

Page 39 of 40
<page_number>39</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

[<u>Directory</u>](Directory)

IFA Coding System for medical devices

IFA CODINGSYSTEM logo

# 10. Appendix B: Dokument history

<table>
  <thead>
    <tr>
        <th>Version</th>
        <th>Date</th>
        <th>Change log</th>
        <th>Document action</th>
    </tr>
  </thead>
  <tbody>
    <tr>
        <td>1.0</td>
        <td>1 July 2019</td>
        <td>Initial Release</td>
        <td> </td>
    </tr>
    <tr>
        <td>1.02</td>
        <td>11 July 2019</td>
        <td>Add, edit</td>
        <td>Appendix A: Errata, layout edited</td>
    </tr>
    <tr>
        <td>1.03</td>
        <td>2 September 2019</td>
        <td>Edit</td>
        <td>Chap. 1: Text modified<br/>Chap. 3.2: Additions HRI representation<br/>Chap. 4: Examples modified, layout edited</td>
    </tr>
    <tr>
        <td>1.03a</td>
        <td>23 October 2019</td>
        <td>Edit</td>
        <td>Chap. 4.5: errta, layout edited</td>
    </tr>
    <tr>
        <td>1.04</td>
        <td>1 July 2020</td>
        <td>Add, edit</td>
        <td>Chap. 2.2: Addition HPC<br/>Chap. 2.4: Addition to Basic UDI-DI<br/>Chap. 3.2: HRI format extended<br/>Chap. 3.3: HRI format for Dokumentation new<br/>Chap. 4: Manufacturer Info EUDAMED new<br/>Chap. 5.5: Example modified</td>
    </tr>
    <tr>
        <td>2.0</td>
        <td>01 July 2021</td>
        <td>Add, edit</td>
        <td>Chap. 3.2.2: Addition HPC information<br/>Chap. 4.1.1: Addition DPM<br/>Chap. 5: Manufacturer Info EUDAMED new<br/>Chap. 6: Examples modified<br/>Chap. 7: Addition HPC examples</td>
    </tr>
    <tr>
        <td>2.1</td>
        <td>14 April 2022</td>
        <td>Edit</td>
        <td>Appendix A: Corrections</td>
    </tr>
    <tr>
        <td>2.2</td>
        <td>07 December 2022</td>
        <td>Add, edit</td>
        <td>Chap. 3.1: Notes on implementation of MDR<br/>requirements supplemented<br/>Chap. 3.2.2.1: CIN order confirmation added<br/>Chap. 3.4: Basic UDI-DI: CIN order confirmation added<br/>Chap. 4.2.4: Identifier for PPN in 9N corrected<br/>Chap. 5: Timeline updated<br/>Chap. 7.2: HRI for DI 9N edited<br/>Appendix A: LOINC added</td>
    </tr>
    <tr>
        <td>2.3</td>
        <td>20 October 2023</td>
        <td>Add, edit</td>
        <td>Chap. 3.1. General added<br/>Chap. 3.4. <em>Master UDI-DI</em> added<br/>Chap . 5. <em>EUDAMED</em> modified<br/>Appendix A: Clinical parameter / LOINC deleted,<br/><em>Master UDI-DI</em> added</td>
    </tr>
    <tr>
        <td>2.4</td>
        <td>13 February 2026</td>
        <td>Add, edit</td>
        <td>Chap. 3.4. <em>Master UDI-DI</em> added<br/>Chap. 8 Examples new</td>
    </tr>
    <tr>
        <td>2.5</td>
        <td>1 April 2026</td>
        <td>Add</td>
        <td>Chap. 3.2.2.1. <em>Structure of the HPC</em> added</td>
    </tr>
  </tbody>
</table>

Figure 21: Document history

Version 2.5

<page_number>Page 40 of 40</page_number>

1 April 2026

Informationsstelle für Arzneispezialitäten – IFA GmbH

<u>[Directory](Directory)</u>

IFA CODINGSYSTEM logo

For additional information on IFA GmbH, the IFA Coding System, the PZN and PPN, the UDI and technical specifications, please visit www.ifaffm.de or contact ifa@ifaffm.de.

The content was created with the greatest care. If you detect errors or are missing content, it is requested that you notify IFA.

The respective laws and regulations are legally binding.

Informationsstelle für Arzneispezialitäten – IFA GmbH

Hamburger Allee 26 – 28

60486 Frankfurt am Main

Germany

phone +49 69 979919-0

ifa@ifaffm.de

www.ifaffm.de/en