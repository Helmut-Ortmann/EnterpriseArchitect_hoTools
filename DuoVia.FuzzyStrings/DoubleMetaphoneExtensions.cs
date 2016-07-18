/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Derived from http://doublemetaphone.googlecode.com/svn/tags/1/DoubleMetaphone.cs 
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
/* Origianal Work Copyright Notice included here as required: 

        Copyright (c) 2008 Anthony Tong Lee

        Permission is hereby granted, free of charge, to any person
        obtaining a copy of this software and associated documentation
        files (the "Software"), to deal in the Software without
        restriction, including without limitation the rights to use,
        copy, modify, merge, publish, distribute, sublicense, and/or sell
        copies of the Software, and to permit persons to whom the
        Software is furnished to do so, subject to the following
        conditions:

        The above copyright notice and this permission notice shall be
        included in all copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
        EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
        OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
        NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
        HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
        WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
        FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
        OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Text;

namespace DuoVia.FuzzyStrings
{
    /// <summary>
    /// DoubleMetaphone string extension
    /// </summary>
    /// <remarks>
    /// Original C++ implementation:
    ///		"Double Metaphone (c) 1998, 1999 by Lawrence Philips"
    ///		http://www.ddj.com/cpp/184401251?pgno=1
    /// </remarks>
    public static class DoubleMetaphoneExtensions
    {
        public static string ToDoubleMetaphone(this string input)
        {
            MetaphoneData metaphoneData = new MetaphoneData();
            int current = 0;

            if (input.Length < 1)
            {
                return input;
            }
            int last = input.Length - 1; //zero based index

            string workingString = input.ToUpperInvariant() + "     ";

            bool isSlavoGermanic = (input.IndexOf(CharW) > -1)
                || (input.IndexOf(CharK) > -1)
                || (input.IndexOf(StrCz, StringComparison.OrdinalIgnoreCase) > -1)
                || (input.IndexOf(StrWitz, StringComparison.OrdinalIgnoreCase) > -1);

            //skip these when at start of word
            if (workingString.StartsWith(StringComparison.OrdinalIgnoreCase, StrGn, StrKn, StrPn, StrWr, StrPs))
            {
                current += 1;
            }

            //Initial 'X' is pronounced 'Z' e.g. 'Xavier'
            if (workingString[0] == CharX)
            {
                metaphoneData.Add(StrS); //'Z' maps to 'S'
                current += 1;
            }

            while ((metaphoneData.PrimaryLength < 4) || (metaphoneData.SecondaryLength < 4))
            {
                if (current >= input.Length)
                {
                    break;
                }

                switch (workingString[current])
                {
                    case CharA:
                    case CharE:
                    case CharI:
                    case CharO:
                    case CharU:
                    case CharY:
                        if (current == 0)
                        {
                            //all init vowels now map to 'A'
                            metaphoneData.Add("A");
                        }
                        current += 1;
                        break;

                    case CharB:
                        //"-mb", e.g", "dumb", already skipped over...
                        metaphoneData.Add("P");

                        if (workingString[current + 1] == CharB)
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        break;

                    case CharAdash:
                        metaphoneData.Add(StrS);
                        current += 1;
                        break;

                    case CharC:
                        //various germanic
                        if ((current > 1)
                            && !IsVowel(workingString[current - 2])
                            && StringAt(workingString, (current - 1), StrAch)
                            && ((workingString[current + 2] != CharI)
                            && ((workingString[current + 2] != CharE)
                            || StringAt(workingString, (current - 2), StrBacher, StrMacher))))
                        {
                            metaphoneData.Add(StrK);
                            current += 2;
                            break;
                        }

                        //special case 'caesar'
                        if ((current == 0) && StringAt(workingString, current, StrCaesar))
                        {
                            metaphoneData.Add(StrS);
                            current += 2;
                            break;
                        }

                        //italian 'chianti'
                        if (StringAt(workingString, current, StrChia))
                        {
                            metaphoneData.Add(StrK);
                            current += 2;
                            break;
                        }

                        if (StringAt(workingString, current, StrCh))
                        {
                            //find 'michael'
                            if ((current > 0) && StringAt(workingString, current, StrChae))
                            {
                                metaphoneData.Add(StrK, StrX);
                                current += 2;
                                break;
                            }

                            //greek roots e.g. 'chemistry', 'chorus'
                            if ((current == 0)
                                && (StringAt(workingString, (current + 1), StrHarac, StrHaris)
                                || StringAt(workingString, (current + 1), StrHor, StrHym, StrHia, StrHem))
                                && !StringAt(workingString, 0, StrChore))
                            {
                                metaphoneData.Add(StrK);
                                current += 2;
                                break;
                            }

                            //germanic, greek, or otherwise 'ch' for 'kh' sound
                            if ((StringAt(workingString, 0, StrVaNsp, StrVoNsp)
                                || StringAt(workingString, 0, StrSch)) // 'architect but not 'arch', 'orchestra', 'orchid'
                                || StringAt(workingString, (current - 2), StrOrches, StrArchit, StrOrchid)
                                || StringAt(workingString, (current + 2), StrT, StrS)
                                    || ((StringAt(workingString, (current - 1), StrA, StrO, StrU, StrE)
                                    || (current == 0)) //e.g., 'wachtler', 'wechsler', but not 'tichner'
                                        && StringAt(workingString, (current + 2), StrL, StrR, StrN, StrM, StrB, StrH, StrF, StrV, StrW, Sp)))
                            {
                                metaphoneData.Add(StrK);
                            }
                            else
                            {
                                if (current > 0)
                                {
                                    if (StringAt(workingString, 0, StrMc))
                                    {
                                        //e.g., "McHugh"
                                        metaphoneData.Add(StrK);
                                    }
                                    else
                                    {
                                        metaphoneData.Add(StrX, StrK);
                                    }
                                }
                                else
                                {
                                    metaphoneData.Add(StrX);
                                }
                            }
                            current += 2;
                            break;
                        }
                        //e.g, 'czerny'
                        if (StringAt(workingString, current, StrCz) && !StringAt(workingString, (current - 2), StrWicz))
                        {
                            metaphoneData.Add(StrS, StrX);
                            current += 2;
                            break;
                        }

                        //e.g., 'focaccia'
                        if (StringAt(workingString, (current + 1), StrCia))
                        {
                            metaphoneData.Add(StrX);
                            current += 3;
                            break;
                        }

                        //double 'C', but not if e.g. 'McClellan'
                        if (StringAt(workingString, current, StrCc) && !((current == 1) && (workingString[0] == CharM)))
                        {
                            //'bellocchio' but not 'bacchus'
                            if (StringAt(workingString, (current + 2), StrI, StrE, StrH)
                                && !StringAt(workingString, (current + 2), StrHu))
                            {
                                //'accident', 'accede' 'succeed'
                                if (((current == 1) && (workingString[current - 1] == CharA))
                                    || StringAt(workingString, (current - 1), StrUccee, StrUcces))
                                {
                                    metaphoneData.Add(StrKs);
                                }
                                //'bacci', 'bertucci', other italian
                                else
                                {
                                    metaphoneData.Add(StrX);
                                }
                                current += 3;
                                break;
                            }
                            else
                            {
                                //Pierce's rule
                                metaphoneData.Add(StrK);
                                current += 2;
                                break;
                            }
                        }

                        if (StringAt(workingString, current, StrCk, StrCg, StrCq))
                        {
                            metaphoneData.Add(StrK);
                            current += 2;
                            break;
                        }

                        if (StringAt(workingString, current, StrCi, StrCe, StrCy))
                        {
                            //italian vs. english
                            if (StringAt(workingString, current, StrCio, StrCie, StrCia))
                            {
                                metaphoneData.Add(StrS, StrX);
                            }
                            else
                            {
                                metaphoneData.Add(StrS);
                            }
                            current += 2;
                            break;
                        }

                        //else
                        metaphoneData.Add(StrK);

                        //name sent in 'mac caffrey', 'mac gregor
                        if (StringAt(workingString, (current + 1), StrspC, StrspQ, StrspG))
                        {
                            current += 3;
                        }
                        else if (StringAt(workingString, (current + 1), StrC, StrK, StrQ)
                            && !StringAt(workingString, (current + 1), StrCe, StrCi))
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        break;

                    case CharD:
                        if (StringAt(workingString, current, StrDg))
                        {
                            if (StringAt(workingString, (current + 2), StrI, StrE, StrY))
                            {
                                //e.g. 'edge'
                                metaphoneData.Add(StrJ);
                                current += 3;
                                break;
                            }
                            else
                            {
                                //e.g. 'edgar'
                                metaphoneData.Add(StrTk);
                                current += 2;
                                break;
                            }
                        }

                        if (StringAt(workingString, current, StrDt, StrDd))
                        {
                            metaphoneData.Add(StrT);
                            current += 2;
                            break;
                        }

                        //else
                        metaphoneData.Add(StrT);
                        current += 1;
                        break;

                    case CharF:
                        if (workingString[current + 1] == CharF)
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        metaphoneData.Add(StrF);
                        break;

                    case CharG:
                        if (workingString[current + 1] == CharH)
                        {
                            if ((current > 0) && !IsVowel(workingString[current - 1]))
                            {
                                metaphoneData.Add(StrK);
                                current += 2;
                                break;
                            }

                            if (current < 3)
                            {
                                //'ghislane', ghiradelli
                                if (current == 0)
                                {
                                    if (workingString[current + 2] == CharI)
                                    {
                                        metaphoneData.Add(StrJ);
                                    }
                                    else
                                    {
                                        metaphoneData.Add(StrK);
                                    }
                                    current += 2;
                                    break;
                                }
                            }
                            //Parker's rule (with some further refinements) - e.g., 'hugh'
                            if (((current > 1) && StringAt(workingString, (current - 2), StrB, StrH, StrD)) //e.g., 'bough'
                                || ((current > 2) && StringAt(workingString, (current - 3), StrB, StrH, StrD)) //e.g., 'broughton'
                                    || ((current > 3) && StringAt(workingString, (current - 4), StrB, StrH)))
                            {
                                current += 2;
                                break;
                            }
                            else
                            {
                                //e.g., 'laugh', 'McLaughlin', 'cough', 'gough', 'rough', 'tough'
                                if ((current > 2) && (workingString[current - 1] == CharU)
                                    && StringAt(workingString, (current - 3), StrC, StrG, StrL, StrR, StrT))
                                {
                                    metaphoneData.Add(StrF);
                                }
                                else if ((current > 0) && workingString[current - 1] != CharI)
                                {
                                    metaphoneData.Add(StrK);
                                }

                                current += 2;
                                break;
                            }
                        }

                        if (workingString[current + 1] == CharN)
                        {
                            if ((current == 1) && IsVowel(workingString[0]) && !isSlavoGermanic)
                            {
                                metaphoneData.Add(StrKn, StrN);
                            }
                            else
                                //not e.g. 'cagney'
                                if (!StringAt(workingString, (current + 2), StrEy)
                                    && (workingString[current + 1] != CharY) && !isSlavoGermanic)
                                {
                                    metaphoneData.Add(StrN, StrKn);
                                }
                                else
                                {
                                    metaphoneData.Add(StrKn);
                                }
                            current += 2;
                            break;
                        }

                        //'tagliaro'
                        if (StringAt(workingString, (current + 1), StrLi) && !isSlavoGermanic)
                        {
                            metaphoneData.Add(StrKl, StrL);
                            current += 2;
                            break;
                        }

                        //-ges-,-gep-,-gel-, -gie- at beginning
                        if ((current == 0)
                            && ((workingString[current + 1] == CharY)
                            || StringAt(workingString, (current + 1), StrEs, StrEp, StrEb, StrEl, StrEy, StrIb, StrIl, StrIn, StrIe, StrEi, StrEr)))
                        {
                            metaphoneData.Add(StrK, StrJ);
                            current += 2;
                            break;
                        }

                        // -ger-,  -gy-
                        if ((StringAt(workingString, (current + 1), StrEr)
                            || (workingString[current + 1] == CharY))
                            && !StringAt(workingString, 0, StrDanger, StrRanger, StrManger)
                            && !StringAt(workingString, (current - 1), StrE, StrI)
                            && !StringAt(workingString, (current - 1), StrRgy, StrOgy))
                        {
                            metaphoneData.Add(StrK, StrJ);
                            current += 2;
                            break;
                        }

                        // italian e.g, 'biaggi'
                        if (StringAt(workingString, (current + 1), StrE, StrI, StrY)
                            || StringAt(workingString, (current - 1), StrAggi, StrOggi))
                        {
                            //obvious germanic
                            if ((StringAt(workingString, 0, StrVaNsp, StrVoNsp)
                                || StringAt(workingString, 0, StrSch))
                                || StringAt(workingString, (current + 1), StrEt))
                            {
                                metaphoneData.Add(StrK);
                            }
                            else
                                //always soft if french ending
                                if (StringAt(workingString, (current + 1), StrIeRsp))
                                {
                                    metaphoneData.Add(StrJ);
                                }
                                else
                                {
                                    metaphoneData.Add(StrJ, StrK);
                                }
                            current += 2;
                            break;
                        }

                        if (workingString[current + 1] == CharG)
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        metaphoneData.Add(StrK);
                        break;

                    case 'H':
                        //only keep if first & before vowel or btw. 2 vowels
                        if (((current == 0) || IsVowel(workingString[current - 1])) && IsVowel(workingString[current + 1]))
                        {
                            metaphoneData.Add(StrH);
                            current += 2;
                        }
                        else //also takes care of 'HH'
                        {
                            current += 1;
                        }
                        break;

                    case 'J':
                        //obvious spanish, 'jose', 'san jacinto'
                        if (StringAt(workingString, current, StrJose) || StringAt(workingString, 0, StrSaNsp))
                        {
                            if (((current == 0) && (workingString[current + 4] == ' ')) || StringAt(workingString, 0, StrSaNsp))
                            {
                                metaphoneData.Add(StrH);
                            }
                            else
                            {
                                metaphoneData.Add(StrJ, StrH);
                            }
                            current += 1;
                            break;
                        }

                        if ((current == 0) && !StringAt(workingString, current, StrJose))
                        {
                            metaphoneData.Add(StrJ, StrA); //Yankelovich/Jankelowicz
                        }
                        else
                            //spanish pron. of e.g. 'bajador'
                            if (current > 0 
                                && IsVowel(workingString[current - 1])
                                && !isSlavoGermanic && ((workingString[current + 1] == CharA)
                                || (workingString[current + 1] == CharO)))
                            {
                                metaphoneData.Add(StrJ, StrH);
                            }
                            else if (current == last)
                            {
                                metaphoneData.Add(StrJ, Sp);
                            }
                            else if (!StringAt(workingString, (current + 1), StrL, StrT, StrK, StrS, StrN, StrM, StrB, StrZ)
                                && !StringAt(workingString, (current - 1), StrS, StrK, StrL))
                            {
                                metaphoneData.Add(StrJ);
                            }

                        if (workingString[current + 1] == CharJ) //it could happen!
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        break;

                    case CharK:
                        if (workingString[current + 1] == CharK)
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        metaphoneData.Add(StrK);
                        break;

                    case CharL:
                        if (workingString[current + 1] == CharL)
                        {
                            //spanish e.g. 'cabrillo', 'gallegos'
                            if (((current == (input.Length - 3))
                                && StringAt(workingString, (current - 1), StrIllo, StrIlla, StrAlle))
                                || ((StringAt(workingString, (last - 1), StrAs, StrOs)
                                || StringAt(workingString, last, StrA, StrO))
                                && StringAt(workingString, (current - 1), StrAlle)))
                            {
                                metaphoneData.Add(StrL, Sp);
                                current += 2;
                                break;
                            }
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        metaphoneData.Add("L");
                        break;

                    case CharM:
                        if ((StringAt(workingString, (current - 1), StrUmb)
                            && (((current + 1) == last)
                            || StringAt(workingString, (current + 2), StrEr))) //'dumb','thumb'
                            || (workingString[current + 1] == CharM))
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        metaphoneData.Add("M");
                        break;

                    case CharN:
                        if (workingString[current + 1] == CharN)
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        metaphoneData.Add(StrN);
                        break;

                    case CharOdash:
                        current += 1;
                        metaphoneData.Add(StrN);
                        break;

                    case CharP:
                        if (workingString[current + 1] == CharH)
                        {
                            metaphoneData.Add(StrF);
                            current += 2;
                            break;
                        }

                        //also account for "campbell", "raspberry"
                        if (StringAt(workingString, (current + 1), StrP, StrB))
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        metaphoneData.Add(StrP);
                        break;

                    case CharQ:
                        if (workingString[current + 1] == CharQ)
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        metaphoneData.Add(StrK);
                        break;

                    case CharR:
                        //french e.g. 'rogier', but exclude 'hochmeier'
                        if ((current == last) && !isSlavoGermanic
                            && StringAt(workingString, (current - 2), StrIe)
                            && !StringAt(workingString, (current - 4), StrMe, StrMa))
                        {
                            metaphoneData.Add(string.Empty, StrR);
                        }
                        else
                        {
                            metaphoneData.Add(StrR);
                        }

                        if (workingString[current + 1] == CharR)
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        break;

                    case CharS:
                        //special cases 'island', 'isle', 'carlisle', 'carlysle'
                        if (StringAt(workingString, (current - 1), StrIsl, StrYsl))
                        {
                            current += 1;
                            break;
                        }

                        //special case 'sugar-'
                        if ((current == 0) && StringAt(workingString, current, StrSugar))
                        {
                            metaphoneData.Add(StrX, StrS);
                            current += 1;
                            break;
                        }

                        if (StringAt(workingString, current, StrSh))
                        {
                            //germanic
                            if (StringAt(workingString, (current + 1), StrHeim, StrHoek, StrHolm, StrHolz))
                            {
                                metaphoneData.Add(StrS);
                            }
                            else
                            {
                                metaphoneData.Add(StrX);
                            }
                            current += 2;
                            break;
                        }

                        //italian & armenian
                        if (StringAt(workingString, current, StrSio, StrSia) || StringAt(workingString, current, StrSian))
                        {
                            if (!isSlavoGermanic)
                            {
                                metaphoneData.Add(StrS, StrX);
                            }
                            else
                            {
                                metaphoneData.Add(StrS);
                            }
                            current += 3;
                            break;
                        }

                        //german & anglicisations, e.g. 'smith' match 'schmidt', 'snider' match 'schneider'
                        //also, -sz- in slavic language altho in hungarian it is pronounced 's'
                        if (((current == 0)
                            && StringAt(workingString, (current + 1), StrM, StrN, StrL, StrW))
                            || StringAt(workingString, (current + 1), StrZ))
                        {
                            metaphoneData.Add(StrS, StrX);
                            if (StringAt(workingString, (current + 1), StrZ))
                            {
                                current += 2;
                            }
                            else
                            {
                                current += 1;
                            }
                            break;
                        }

                        if (StringAt(workingString, current, StrSc))
                        {
                            //Schlesinger's rule
                            if (workingString[current + 2] == CharH)
                            {
                                //dutch origin, e.g. 'school', 'schooner'
                                if (StringAt(workingString, (current + 3), StrOo, StrEr, StrEn, StrUy, StrEd, StrEm))
                                {
                                    //'schermerhorn', 'schenker'
                                    if (StringAt(workingString, (current + 3), StrEr, StrEn))
                                    {
                                        metaphoneData.Add(StrX, StrSk);
                                    }
                                    else
                                    {
                                        metaphoneData.Add(StrSk);
                                    }
                                    current += 3;
                                    break;
                                }
                                else
                                {
                                    if ((current == 0) && !IsVowel(workingString[3]) && (workingString[3] != CharW))
                                    {
                                        metaphoneData.Add(StrX, StrS);
                                    }
                                    else
                                    {
                                        metaphoneData.Add(StrX);
                                    }
                                    current += 3;
                                    break;
                                }
                            }

                            if (StringAt(workingString, (current + 2), StrI, StrE, StrY))
                            {
                                metaphoneData.Add(StrS);
                                current += 3;
                                break;
                            }
                            //else
                            metaphoneData.Add(StrSk);
                            current += 3;
                            break;
                        }

                        //french e.g. 'resnais', 'artois'
                        if ((current == last) && StringAt(workingString, (current - 2), StrAi, StrOi))
                        {
                            metaphoneData.Add(string.Empty, StrS);
                        }
                        else
                        {
                            metaphoneData.Add(StrS);
                        }

                        if (StringAt(workingString, (current + 1), StrS, StrZ))
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        break;

                    case CharT:
                        if (StringAt(workingString, current, StrTion))
                        {
                            metaphoneData.Add(StrX);
                            current += 3;
                            break;
                        }

                        if (StringAt(workingString, current, StrTia, StrTch))
                        {
                            metaphoneData.Add(StrX);
                            current += 3;
                            break;
                        }

                        if (StringAt(workingString, current, StrTh) || StringAt(workingString, current, StrTth))
                        {
                            //special case 'thomas', 'thames' or germanic
                            if (StringAt(workingString, (current + 2), StrOm, StrAm)
                                || StringAt(workingString, 0, StrVaNsp, StrVoNsp) || StringAt(workingString, 0, StrSch))
                            {
                                metaphoneData.Add(StrT);
                            }
                            else
                            {
                                metaphoneData.Add(StrO, StrT);
                            }
                            current += 2;
                            break;
                        }

                        if (StringAt(workingString, (current + 1), StrT, StrD))
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        metaphoneData.Add(StrT);
                        break;

                    case CharV:
                        if (workingString[current + 1] == CharV)
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        metaphoneData.Add(StrF);
                        break;

                    case CharW:
                        //can also be in middle of word
                        if (StringAt(workingString, current, StrWr))
                        {
                            metaphoneData.Add(StrR);
                            current += 2;
                            break;
                        }

                        if ((current == 0) && (IsVowel(workingString[current + 1])
                            || StringAt(workingString, current, StrWh)))
                        {
                            //Wasserman should match Vasserman
                            if (IsVowel(workingString[current + 1]))
                            {
                                metaphoneData.Add(StrA, StrF);
                            }
                            else
                            {
                                //need Uomo to match Womo
                                metaphoneData.Add(StrA);
                            }
                        }

                        //Arnow should match Arnoff
                        if ((current == last && current > 0 && IsVowel(workingString[current - 1]))
                            || StringAt(workingString, (current - 1), StrEwski, StrEwsky, StrOwski, StrOwsky)
                            || StringAt(workingString, 0, StrSch))
                        {
                            metaphoneData.Add(string.Empty, StrF);
                            current += 1;
                            break;
                        }

                        //polish e.g. 'filipowicz'
                        if (StringAt(workingString, current, StrWicz, StrWitz))
                        {
                            metaphoneData.Add(StrTs, StrFx);
                            current += 4;
                            break;
                        }

                        //else skip it
                        current += 1;
                        break;

                    case CharX:
                        //french e.g. breaux
                        if (!((current == last)
                            && (StringAt(workingString, (current - 3), StrIau, StrEau)
                            || StringAt(workingString, (current - 2), StrAu, StrOu))))
                        {
                            metaphoneData.Add(StrKs);
                        }

                        if (StringAt(workingString, (current + 1), StrC, StrX))
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        break;

                    case CharZ:
                        //chinese pinyin e.g. 'zhao'
                        if (workingString[current + 1] == CharH)
                        {
                            metaphoneData.Add(StrJ);
                            current += 2;
                            break;
                        }
                        else if (StringAt(workingString, (current + 1), StrZo, StrZi, StrZa)
                            || (isSlavoGermanic && ((current > 0) && workingString[current - 1] != CharT)))
                        {
                            metaphoneData.Add(StrS, StrTs);
                        }
                        else
                        {
                            metaphoneData.Add(StrS);
                        }

                        if (workingString[current + 1] == CharZ)
                        {
                            current += 2;
                        }
                        else
                        {
                            current += 1;
                        }
                        break;

                    default:
                        current += 1;
                        break;
                }
            }

            return metaphoneData.ToString();
        }


        static bool IsVowel(this char self)
        {
            return (self == CharA) || (self == CharE) || (self == CharI)
                || (self == CharO) || (self == CharU) || (self == CharY);
        }

        private const char CharA = 'A';
        private const char CharW = 'W';
        private const char CharK = 'K';
        private const string StrCz = "CZ";
        private const string StrWitz = "WITZ";
        private const string StrGn = "GN";
        private const string StrKn = "KN";
        private const string StrPn = "PN";
        private const string StrWr = "WR";
        private const string StrPs = "PS";
        private const char CharX = 'X';
        private const string StrS = "S";
        private const char CharE = 'E';
        private const char CharI = 'I';
        private const char CharO = 'O';
        private const char CharU = 'U';
        private const char CharY = 'Y';
        private const char CharB = 'B';
        private const char CharAdash = 'Ã';
        private const string StrAch = "ACH";
        private const string StrBacher = "BACHER";
        private const string StrMacher = "MACHER";
        private const string StrK = "K";
        private const string StrCaesar = "CAESAR";
        private const string StrChia = "CHIA";
        private const string StrCh = "CH";
        private const string StrChae = "CHAE";
        private const string StrX = "X";
        private const string StrHarac = "HARAC";
        private const string StrHaris = "HARIS";
        private const string StrHor = "HOR";
        private const string StrHym = "HYM";
        private const string StrHia = "HIA";
        private const string StrHem = "HEM";
        private const string StrChore = "CHORE";
        private const string StrVaNsp = "VAN ";
        private const string StrVoNsp = "VON ";
        private const string StrSch = "SCH";
        private const string StrOrches = "ORCHES";
        private const string StrArchit = "ARCHIT";
        private const string StrOrchid = "ORCHID";
        private const string StrT = "T";
        private const string StrA = "A";
        private const string StrO = "O";
        private const string StrU = "U";
        private const string StrE = "E";
        private const string StrL = "L";
        private const string StrR = "R";
        private const string StrN = "N";
        private const string StrM = "M";
        private const string StrB = "B";
        private const string StrH = "H";
        private const string StrF = "F";
        private const string StrV = "V";
        private const string StrW = "W";
        private const string Sp = " ";
        private const string StrMc = "MC";
        private const string StrWicz = "WICZ";
        private const string StrCia = "CIA";
        private const string StrCc = "CC";
        private const char CharM = 'M';
        private const string StrI = "I";
        private const string StrHu = "HU";
        private const string StrUccee = "UCCEE";
        private const string StrUcces = "UCCES";
        private const string StrKs = "KS";
        private const string StrCk = "CK";
        private const string StrCg = "CG";
        private const string StrCq = "CQ";
        private const string StrCi = "CI";
        private const string StrCe = "CE";
        private const string StrCy = "CY";
        private const string StrCio = "CIO";
        private const string StrCie = "CIE";
        private const string StrspC = " C";
        private const string StrspQ = " Q";
        private const string StrspG = " G";
        private const string StrC = "C";
        private const string StrQ = "Q";
        private const char CharC = 'C';
        private const char CharD = 'D';
        private const string StrDg = "DG";
        private const string StrY = "Y";
        private const string StrJ = "J";
        private const string StrTk = "TK";
        private const string StrDt = "DT";
        private const string StrDd = "DD";
        private const char CharF = 'F';
        private const char CharG = 'G';
        private const char CharH = 'H';
        private const string StrD = "D";
        private const string StrG = "G";
        private const char CharN = 'N';
        private const string StrEy = "EY";
        private const string StrLi = "LI";
        private const string StrKl = "KL";
        private const string StrEs = "ES";
        private const string StrEp = "EP";
        private const string StrEb = "EB";
        private const string StrEl = "EL";
        private const string StrIb = "IB";
        private const string StrIl = "IL";
        private const string StrIn = "IN";
        private const string StrIe = "IE";
        private const string StrEi = "EI";
        private const string StrEr = "ER";
        private const string StrDanger = "DANGER";
        private const string StrRanger = "RANGER";
        private const string StrManger = "MANGER";
        private const string StrRgy = "RGY";
        private const string StrOgy = "OGY";
        private const string StrAggi = "AGGI";
        private const string StrOggi = "OGGI";
        private const string StrIeRsp = "IER ";
        private const string StrJose = "JOSE";
        private const string StrSaNsp = "SAN ";
        private const string StrZ = "Z";
        private const char CharJ = 'J';
        private const char CharL = 'L';
        private const string StrIllo = "ILLO";
        private const string StrIlla = "ILLA";
        private const string StrAlle = "ALLE";
        private const string StrAs = "AS";
        private const string StrOs = "OS";
        private const string StrUmb = "UMB";
        private const char CharOdash = 'Ð';
        private const char CharP = 'P';
        private const string StrP = "P";
        private const char CharQ = 'Q';
        private const string StrMe = "ME";
        private const string StrMa = "MA";
        private const char CharR = 'R';
        private const char CharS = 'S';
        private const string StrIsl = "ISL";
        private const string StrYsl = "YSL";
        private const string StrSugar = "SUGAR";
        private const string StrSh = "SH";
        private const string StrHeim = "HEIM";
        private const string StrHoek = "HOEK";
        private const string StrHolm = "HOLM";
        private const string StrHolz = "HOLZ";
        private const string StrSio = "SIO";
        private const string StrSia = "SIA";
        private const string StrSian = "SIAN";
        private const string StrSc = "SC";
        private const string StrOo = "OO";
        private const string StrEn = "EN";
        private const string StrUy = "UY";
        private const string StrEd = "ED";
        private const string StrEm = "EM";
        private const string StrSk = "SK";
        private const string StrAi = "AI";
        private const string StrOi = "OI";
        private const string StrTion = "TION";
        private const string StrTia = "TIA";
        private const string StrTch = "TCH";
        private const char CharT = 'T';
        private const string StrTh = "TH";
        private const string StrTth = "TTH";
        private const string StrOm = "OM";
        private const string StrAm = "AM";
        private const char CharV = 'V';
        private const string StrWh = "WH";
        private const string StrEwski = "EWSKI";
        private const string StrEwsky = "EWSKY";
        private const string StrOwski = "OWSKI";
        private const string StrOwsky = "OWSKY";
        private const string StrFx = "FX";
        private const string StrTs = "TS";
        private const string StrEau = "EAU";
        private const string StrIau = "IAU";
        private const string StrAu = "AU";
        private const string StrOu = "OU";
        private const char CharZ = 'Z';
        private const string StrZa = "ZA";
        private const string StrZi = "ZI";
        private const string StrZo = "ZO";
        private const string StrEt = "ET";


        static bool StartsWith(this string self, StringComparison comparison, params string[] strings)
        {
            foreach (string str in strings)
            {
                if (self.StartsWith(str, comparison))
                {
                    return true;
                }
            }
            return false;
        }

        static bool StringAt(this string self, int startIndex, params string[] strings)
        {
            if (startIndex < 0)
            {
                startIndex = 0;
            }

            foreach (string str in strings)
            {
                if (self.IndexOf(str, startIndex, StringComparison.OrdinalIgnoreCase) >= startIndex)
                {
                    return true;
                }
            }
            return false;
        }


        private class MetaphoneData
        {
            readonly StringBuilder _primary = new StringBuilder(5);
            readonly StringBuilder _secondary = new StringBuilder(5);


            #region Properties

            internal bool Alternative { get; set; }
            internal int PrimaryLength
            {
                get
                {
                    return _primary.Length;
                }
            }

            internal int SecondaryLength
            {
                get
                {
                    return _secondary.Length;
                }
            }

            #endregion


            internal void Add(string main)
            {
                if (main != null)
                {
                    _primary.Append(main);
                    _secondary.Append(main);
                }
            }

            internal void Add(string main, string alternative)
            {
                if (main != null)
                {
                    _primary.Append(main);
                }

                if (alternative != null)
                {
                    Alternative = true;
                    if (alternative.Trim().Length > 0)
                    {
                        _secondary.Append(alternative);
                    }
                }
                else
                {
                    if (main != null && main.Trim().Length > 0)
                    {
                        _secondary.Append(main);
                    }
                }
            }

            public override string ToString()
            {
                string ret = (Alternative ? _secondary : _primary).ToString();
                //only give back 4 char metaph
                if (ret.Length > 4)
                {
                    ret = ret.Substring(0, 4);
                }

                return ret;
            }
        }
    }
}
