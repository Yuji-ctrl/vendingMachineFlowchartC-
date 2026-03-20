using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading;

class VendingMachine
{
    static Random rand = new Random();

    static Dictionary<string, int> products = new Dictionary<string, int>()
    {
        {"エナジードリンク", 200 },
        {"コーラ", 190 },
        {"お茶", 160 },
        {"水", 100 }
    };

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("商品一覧:");
            foreach (var item in products)
            {
                Console.WriteLine($"{item.Key} - {item.Value}円");
            }
            Console.Write("購入したい商品名を入力してください (終了はexit): ");
            string choice = Console.ReadLine();

            if (choice == "exit") break;

            if (!products.ContainsKey(choice))
            {
                Console.WriteLine("商品が見つかりません。");
                continue;
            }

            int price = products[choice];
            Console.WriteLine($"{choice} を {price}円 で購入します。支払い方法は現金(cash)か電子マネー(ecard)を入力してください：");
            string payment = Console.ReadLine();

            if (payment == "cash")
            {
                PlaySound("自販機-お金投入.wav");
                Console.WriteLine("現金支払いを受け付けました。");

                // お釣りがある場合の処理(簡略化、1000円投入想定)
                int inserted = 1000;
                int change = inserted - price;
                if (change > 0)
                {
                    Console.WriteLine($"お釣り：{change}円");
                    PlaySound("自販機-お釣り音.wav");
                }
            }
            else if (payment == "ecard")
            {
                Console.Beep(1000, 200);
                Console.WriteLine("電子マネー支払い完了");
            }
            else
            {
                Console.WriteLine("無効な支払い方法です。");
                continue;
            }

            string ticket = DrawTicket();
            Console.WriteLine($"くじ引き結果: {ticket}");
            if (IsWinner(ticket))
            {
                Console.WriteLine("おめでとうございます！もう一本買えます！");
            }
            else
            {
                Console.WriteLine("ざんねん！また挑戦してね！");
            }
                Console.WriteLine();
        }
    }

    static void PlaySound(string filename)
    {
        try
        {
            using (SoundPlayer player = new SoundPlayer(filename))
            {
                player.Load();
                player.PlaySync();
            }
        }
        catch
        {
            Console.WriteLine("音声ファイル再生エラー");
        }
    }

    static string DrawTicket()
    {
        string firstThree = rand.Next(100, 1000).ToString();
        char lastDigit;

        // 4桁目は80%の確率で外れ（前の数字と異なる）
        if (rand.NextDouble() < 0.8)
        {
            do
            {
                lastDigit = (char)rand.Next('0', '9' + 1);
            } while (lastDigit == firstThree[2]);
        }
        else
        {
            lastDigit = firstThree[2];
        }

        string ticket = firstThree + lastDigit;

        AnimateNumber(ticket);
        return ticket;
    }

    static void AnimateNumber(string number)
    {
        foreach (char c in number)
        {
            Console.Write(c);
            Thread.Sleep(300);
        }
        Console.WriteLine();
    }

    static bool IsWinner(string ticket)
    {
        // 4桁全て同じ数字の場合は当たり
        if (ticket.Distinct().Count() == 1)
            return true;

        return false;
        // それ以外は確率で判定
        //return rand.NextDouble() < 0.3;
    }

}
