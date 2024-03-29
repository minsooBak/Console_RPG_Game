﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RPG_Game
{
    internal class CreateCharacter
    {
        public string? CreatePlayer()
        {
            while (true)
            {
                Console.WriteLine("RPG_GAME에 오신것을 환영합니다!");
                Console.WriteLine("이곳은 캐릭터 생성을 하는 곳 입니다.\n");
                Console.WriteLine("1. 캐릭터 생성");
                Console.WriteLine("\n0. 종료\n");
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");
                string? str = Console.ReadLine();
                if (str != null && int.TryParse(str, out int a))
                {
                    int type = int.Parse(str);
                    if (type == 0)
                    {
                        Console.Clear();
                        return null;
                    }
                    else if (type == 1)
                    {
                        Console.Clear();
                        string name = CreateName();
                        Console.Clear();
                        string job = CreateJob();
                        Console.Clear();
                        return name + " " + job;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다!");
                        Console.WriteLine("===================================================");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다!");
                    Console.WriteLine("===================================================");
                }
            }
        }
        string CreateName()
        {
            while (true)
            {
                Utilities.TextColor("캐릭터 생성 - 이름", ConsoleColor.DarkYellow, ConsoleColor.Gray);
                Console.WriteLine("이름을 정해주세요!\n");
                Console.WriteLine("당신의 이름은? [이름 생성 규칙 : 띄워쓰기 금지 / 10글자 이내]");
                Console.Write(">>");
                string? str = Console.ReadLine();
                bool isCheck = Regex.IsMatch(str, @"[^a-zA-Z0-9가-힣]");
                if (str != null && str.Length <= 10 && isCheck == false)
                {
                    return str;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 이름입니다!");
                    Console.WriteLine("===================================================");
                }
            }
        }

        string CreateJob()
        {
            while (true)
            {
                Utilities.TextColor("캐릭터 생성 - 직업", ConsoleColor.DarkYellow, ConsoleColor.Gray);
                Console.WriteLine("직업을 정해주세요!\n");
                Console.WriteLine("1. 전사");
                Console.WriteLine("2. 마법사");
                Console.WriteLine("3. 궁수");
                Console.WriteLine("4. 도적");
                Console.WriteLine("당신의 직업은?");
                Console.Write(">>");
                string? str = Console.ReadLine();

                if (str != null && int.TryParse(str, out int a))
                {
                    int type = int.Parse(str);
                    if (type > 0 && type < 5)
                    {
                        switch (type)
                        {
                            case 1: return "전사";
                            case 2: return "마법사";
                            case 3: return "궁수";
                            case 4: return "도적";
                        }
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 이름입니다!");
                        Console.WriteLine("===================================================");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 이름입니다!");
                    Console.WriteLine("===================================================");
                }
            }
            return "";
        }
    }
}
