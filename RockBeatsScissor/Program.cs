// Variável que guarda as estatísticas dos usuários
Dictionary<string, (int victory, int draw, int defeat)> users = new();

// Variável que guarda as opções do menu
string[] mainMenu = new string[]{
        "📖 Tutorial",
        "🎮 Jogar Agora",
        "📃 Adicionar Usuário",
        "🏆 Ranking",
        "↖️ Sair"
};

// Variável que guarda as opções de jogo
string[] gameOptions = new string[]{
    "Pedra",
    "Papel",
    "Tesoura"
};

// Variável que guarda as regras do jogo
Dictionary<string, string> rules = new Dictionary<string, string>
{
    { "Pedra", "Tesoura" },
    { "Papel", "Pedra" },
    { "Tesoura", "Papel" }
};

// Variável que guarda o usuário escolhido
string user;


/// <summary>
/// Este método inicializa o programa sendo o método principal
/// </summary>
void Welcome()
{
    Console.Clear();

    // Imprimindo o nome do programa
    Console.WriteLine("""
       ___         _                 ___                 _          _____                                
      / _ \___  __| |_ __ __ _      / _ \__ _ _ __   ___| |   ___  /__   \___  ___  ___  _   _ _ __ __ _ 
     / /_)/ _ \/ _` | '__/ _` |    / /_)/ _` | '_ \ / _ \ |  / _ \   / /\/ _ \/ __|/ _ \| | | | '__/ _` |
    / ___/  __/ (_| | | | (_| |_  / ___/ (_| | |_) |  __/ | |  __/  / / |  __/\__ \ (_) | |_| | | | (_| |
    \/    \___|\__,_|_|  \__,_( ) \/    \__,_| .__/ \___|_|  \___|  \/   \___||___/\___/ \__,_|_|  \__,_|
                              |/             |_|                                                         
    """);

    // Fazendo o usuário escolher as opções do menu
    int option = ChooseOptions(mainMenu);

    // Enquanto a opção não for a (5) Sair, continuar o programa
    while (option != 5)
    {
        // Validar a opção para executar a função relacionada a ela
        Choice(option);

        // Fazer o usuário escolher uma nova opção
        option = ChooseOptions(mainMenu);
    }
}


/// <summary>
/// Este método lista ao ranking de usuários
/// </summary>
void ShowRanking()
{
    // Se não houver usuários
    if (users.Count == 0) {
        Console.WriteLine("Nenhum usuário foi adicionado ainda!");
    } else 
    {
        // Listando estatísticas
        Console.WriteLine();
        Console.WriteLine("🏆 Ranking 🏆");

        int pointsPerVictory = 3;
        int pointsPerDraw = 1;
        int pointsPerDefeat = 0;

        var ranking = users
            .Select(user => new {
                Name = user.Key,
                Score = (user.Value.victory * pointsPerVictory) + (user.Value.draw * pointsPerDraw) + (user.Value.defeat * pointsPerDefeat)
            })
            .OrderByDescending(user => user.Score)
            .ToList();

        int position = 1;
        foreach (var user in ranking) 
        {
            string message = "";
            string unity = "ponto";
            switch (position) {
                case 1:  message += "🥇"; break;
                case 2:  message += "🥈"; break;
                case 3:  message += "🥉"; break;
                default: message += position; break;
            }
            if (user.Score > 1) {
                unity = "pontos";
            }
            message += $" {user.Name}: {user.Score} {unity}";
            Console.WriteLine(message);
            position++;
        }
    }
}


/// <summary>
/// Este método inicializa a partida do jogo
/// </summary>
void Play()
{
    Console.WriteLine();
    Console.WriteLine("🎮 Vamos jogar 🎮");

    // Se não houver nenhum usuário na base de dados do programa...
    if (users.Count == 0)
    {
        // Adicione um novo usuário
        AddUser();
    }

    // Escolhendo um usuário para jogar
    ChooseUser();

    // Escolhendo a opção da mão 1 e 2
    Console.WriteLine("Mão 1:");
    int firstHand = ChooseOptions(gameOptions);
    Console.WriteLine("Mão 2:");
    int secondHand = ChooseOptions(gameOptions);

    // Guardando as opções em um dicionário para facilitar o acesso
    var playerOptions = new Dictionary<int, string>
    {
        { 1, gameOptions[firstHand - 1] },
        { 2, gameOptions[secondHand - 1] }
    };

    // Escolhendo as opções da mão 1 e 2 do programa (computador)
    Random rand = new Random();
    int pcFirstHand = rand.Next(gameOptions.Length); // Indice aleatório entre 0 e 2
    int pcSecondHand = rand.Next(gameOptions.Length); // Indice aleatório entre 0 e 2

    var pcOptions = new Dictionary<int, string>
    {
        { 1, gameOptions[pcFirstHand] },
        { 2, gameOptions[pcSecondHand] }
    };

    // Mostrando as opções das mãos 1 e 2 do jogador e do computador
    ShowOptions(playerOptions.Values.ToArray());
    ShowOptions(pcOptions.Values.ToArray(), true);

    // Escolhendo a melhor opção para ganhar do programa
    Console.WriteLine("Escolha a sua melhor opção para ganhar de mim!");
    int choice = ChooseOptions(playerOptions.Values.ToArray());

    // Fazendo o programa escolher a melhor opção para ganhar do jogador
    int pcChoice = PcAvaliationOptions(playerOptions, pcOptions);

    // Pegando o nome das opções que o player escolheu e o computador escolheu
    string choiceName = playerOptions[choice];
    string pcChoiceName = pcOptions[pcChoice];
    Console.WriteLine($"Você escolheu: {GetEmoji(choiceName)} {choiceName}");
    Console.WriteLine($"Eu escolhi: {GetEmoji(pcChoiceName)} {pcChoiceName}");

    // Validando as opções
    // Se forem iguals será empate
    if (choiceName == pcOptions[pcChoice])
    {
        Console.WriteLine("Empatamos!");
        users[user] = (users[user].victory, users[user].draw + 1, users[user].defeat);
    } else // Se não
    {   
        // Se o player ganhar
        if (PlayerVictory(choiceName, pcChoiceName))
        {
            Console.WriteLine("Você ganhou!");
            users[user] = (users[user].victory + 1, users[user].draw, users[user].defeat);
        } else
        {
            Console.WriteLine("Eu ganhei!");
            users[user] = (users[user].victory, users[user].draw, users[user].defeat + 1);
        }
    }
}


/// <summary>
/// Este método retorna o emoji de acordo com a opção passada
/// </summary>
/// <param name="opt"> Este parametro representa a opção escolhida</param>
/// <returns>Retorna uma string com o emoji</returns>
string GetEmoji(string opt) 
{
    switch (opt) {
        case "Pedra": return "✊";
        case "Papel": return "✋";
        case "Tesoura": return "✌️";
        default: return "";
    }
}


/// <summary>
/// Este método verifica se o plaver venceu a partida
/// </summary>
/// <param name="playerChoice">Escolha do jogador</param>
/// <param name="pcChoice">Escolha do jogador</param>
/// <returns>O resultado da análise se o player venceu a partida</returns>
bool PlayerVictory(string playerChoice, string pcChoice)
{
    // Verificando se o player venceu
    if (rules[playerChoice] == pcChoice)
    {

        // Returnar true se ele venceu
        return true;
    }
    // Retornar false se ele não venceu
    return false;
}


/// <summary>
/// Este método é o algorítimo para avaliar qual é a melhor opção de escolha
/// </summary>
/// <param name="playerOptions">Opções de escolhas do jogador</param>
/// <param name="pcOptions">Opções de escolhar do algorítimo</param>
/// <returns>Retorna a melhor escolha que o algorítimo decidiu</returns>
int PcAvaliationOptions(Dictionary<int, string> playerOptions, Dictionary<int, string> pcOptions)
{
    // Percorrendo as opções que o algorítimo tem
    foreach (var pcOption in pcOptions.Values)
    {
        // Percorrendo as opções que o player tem
        foreach (var playerOption in playerOptions.Values)
        {
            // Verificando as possibilidades de vitória
            if (rules[pcOption] == playerOption)
            {
                // Retornando a opção que pode vencer o player
                return pcOptions.FirstOrDefault(option => option.Value == pcOption).Key;
            }
        }
    }

    // Se não houver uma vitória clara, o algorítimo escolhe aleatoriamente
    return pcOptions.Keys.ElementAt(new Random().Next(pcOptions.Count));
}


/// <summary>
/// Este método mostra as opções que tem em mãos
/// <summary/>
/// <param name="options">Opções para listar</param>
/// <param name="pc">Este parametro marca se estamos listando as opções para o computador ou para o usuário</param>
void ShowOptions(string[] options, bool pc = false)
{   
    Console.WriteLine();
    // Se não for o pc
    if (!pc)
    {
        Console.WriteLine("Suas opções são:");
    }
    else // Se não
    {
        Console.WriteLine("Minhas opções são:");
    }

    // Mostrando as opções
    int hand = 1;
    foreach (string option in options)
    {       
        Console.WriteLine($"Mão {hand} - {GetEmoji(option)} {option}");
        hand++;
    }
}


/// <summary>
/// Este método possibilita escolher o usuário para jogar
/// <summary/>
void ChooseUser()
{
    Console.WriteLine();
    Console.WriteLine("Com qual usuário você quer jogar ?");

    // Listando os usuários
    string[] keys = users.Keys.ToArray();
    int option = ChooseOptions(keys);

    // Atribuindo à variável user o usuário escolhido
    user = keys[option - 1];
}


/// <summary>
/// Este método ensina o player como o jogo funciona
/// </summary>
void Tutorial()
{
    Console.WriteLine();
    Console.WriteLine("📖 Tutorial 📖");
    Console.WriteLine("Cada jogador usa as duas mãos, jogando duas vezes (uma com cada mão) e recebendo valores de pedra, papel ou tesoura. Depois, escolhe um dos valores para enfrentar o adversário. O vencedor é decidido pelas regras tradicionais: pedra vence tesoura, tesoura vence papel e papel vence pedra.");
}


/// <summary>
/// Este método valida a opção escolhida e dependendo dela executa a função atrelada a ela
/// <param name="option">Este parametro representa a opção</param>
void Choice(int option)
{
    switch (option)
    {
        case 1: Tutorial(); break;
        case 2: Play(); break;
        case 3: AddUser(); break;
        case 4: ShowRanking(); break;
        case 5: Console.WriteLine("Adeus!"); break;
    }
}


///<summary>
///Este método cadastra o funcionário no sistema
///</summary>
void AddUser()
{
    Console.WriteLine();
    Console.WriteLine("📃 Adicionar Usuário 📃");

    // Pegando o nome do usuário
    string name = GetName();

    // Caso o usuário existir
    while (users.ContainsKey(name))
    {
        Console.WriteLine("Este usuário já foi adicionado!");
        name = GetName();
    }

    // Adicionar usuário
    users.Add(name, (0, 0, 0));
}


/// <summary>
/// Este método imprime o menu para escolher as opções
/// </summary>
/// <param name="validOptions">Este parametro representa as opções válidas para a validação da opção escolhida</param>
/// <returns>Retorna a opção escolhida</returns>
int ChooseOptions(string[] validOptions)
{
    Console.WriteLine();
    // Criando a variável para guardar as opções
    Dictionary<int, string> options = new();

    // Percorrendo todas as opções passadas para adicionar à variável de opções
    int number = 0;
    foreach (string opt in validOptions)
    {
        options.Add(number++, opt);
        Console.WriteLine($"({number}) {GetEmoji(opt)} {opt}");
    }
    Console.WriteLine("Escolha uma opção:");

    // Escolhendo uma opçâo
    string? option = Console.ReadKey().KeyChar.ToString();
    Console.WriteLine();

    // Validando se a opção é válida
    bool valid = ValidateOpt(option, number);
    while (!valid)
    {
        Console.WriteLine("Opção inválida:");
        option = Console.ReadKey().KeyChar.ToString();
        Console.WriteLine();
        valid = ValidateOpt(option, number);
    }

    // retornando a opção escolhida
    return int.Parse(option);

}


/// <summary>
/// Este método valalida a opção para ver se ela está dentro dos padrões
/// </summary>
/// <param name="option"> Este parametro é a opção escolhida para a validação </param>
/// <param name="max"> Este parametro é a opção máxima e é usado para saber se a opção escolhida é maior que ele, se for a opção escolhida será inválida </param>
/// <returns>retorna o valor da análise, se for true a opção é válida, se for false a opção é inválida</returns>
bool ValidateOpt(string option, int max)
{
    // Se a opção não for nula
    if (option != null)
    {
        // Verificando se a opção está dentro das passadas
        int numOpt = int.Parse(option);
        if (numOpt > 0 && numOpt <= max)
        {
            return true;
        }
    }
    return false;
}


/// <summary>
/// Este método pega o nome do usuário e valida para ver se ele está dentro dos padrões
/// </summary>
/// <returns> Este método retornará o nome digitado </returns>
string GetName()
{
    Console.WriteLine("Qual o seu nome?");

    // Lendo o nome
    string? name = Console.ReadLine();
    Console.WriteLine();

    // Se o nome for nulo
    while (name == null)
    {
        Console.WriteLine("Seu nome não pode ser nulo!");
        name = Console.ReadLine();
        Console.WriteLine();
    }
    return name;
}


// Inicializando o programa
Welcome();