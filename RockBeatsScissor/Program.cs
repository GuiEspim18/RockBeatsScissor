// Variável que guarda as estatísticas dos usuários
Dictionary<string, (int, int, int)> statitics = new();

// Variável que guarda as opções do menu
string[] mainMenu = new string[]{
        "Tutorial",
        "Jogar Agora",
        "Adicionar Usuário",
        "Estatísticas",
        "Sair"
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


// Função que inicia o programa
void Welcome()
{
    Console.Clear();

    // Imprimindo o nome do programa
    Console.WriteLine("--Pedra, Papel e Tesoura--");

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


// Função para jogar o jogo
void Play()
{
    Console.WriteLine("--Vamos jogar--");

    // Se não houver nenhum usuário na base de dados do programa...
    if (statitics.Count == 0)
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
    Console.WriteLine($"Eu escolhi: {pcChoiceName}");

    // Validando as opções
    // Se forem iguals será empate
    if (choiceName == pcOptions[pcChoice])
    {
        Console.WriteLine("Empatamos!");
    } else // Se não
    {   
        // Se o player ganhar
        if (PlayerVictory(choiceName, pcChoiceName))
        {
            Console.WriteLine("Você ganhou!");
        } else
        {
            Console.WriteLine("Eu ganhei!");
        }
    }
}

// Função para verificar se o player venceu ou ganhou
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

// Função para o algorítimo avaliar qual opção é a melhor
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


// Função para mostrar as opções que tem em mãos
void ShowOptions(string[] options, bool pc = false)
{   
    // Se não for o pc
    if (!pc)
    {
        Console.WriteLine("As suas opções são:");
    }
    else // Se não
    {
        Console.WriteLine("As minhas opções são:");
    }

    // Mostrando as opções
    int hand = 1;
    foreach (string option in options)
    {
        Console.WriteLine($"Mão {hand} - {option}");
        hand++;
    }
}


// Função para escolher o usuário para jogar
void ChooseUser()
{
    Console.WriteLine("Com qual usuário você quer jogar ?");

    // Listando os usuários
    string[] keys = statitics.Keys.ToArray();
    int option = ChooseOptions(keys);

    // Atribuindo à variável user o usuário escolhido
    user = keys[option - 1];
}

// Função para ensinar o player como funciona o jogo
void Tutorial()
{
    Console.WriteLine("--Tutorial--");
    Console.WriteLine("Cada jogador usa as duas mãos, jogando duas vezes (uma com cada mão) e recebendo valores de pedra, papel ou tesoura. Depois, escolhe um dos valores para enfrentar o adversário. O vencedor é decidido pelas regras tradicionais: pedra vence tesoura, tesoura vence papel e papel vence pedra.");
}

// Função para validar a opção escolhida
void Choice(int option)
{
    switch (option)
    {
        case 1: Tutorial(); break;
        case 2: Play(); break;
        case 3: AddUser(); break;
        case 4: Console.WriteLine(""); break;
        case 5: Console.WriteLine(""); break;
    }
}

// Função para adicionar o usuário
void AddUser()
{
    Console.WriteLine("--Adicionar Usuário--");

    // Pegando o nome do usuário
    string name = GetName();

    // Caso o usuário existir
    while (statitics.ContainsKey(name))
    {
        Console.WriteLine("Este usuário já foi adicionado!");
        name = GetName();
    }

    // Adicionar usuário
    statitics.Add(name, (0, 0, 0));
}

// Função que imprime um menu para escolher as opções
int ChooseOptions(string[] validOptions)
{
    // Criando a variável para guardar as opções
    Dictionary<int, string> options = new();

    // Percorrendo todas as opções passadas para adicionar à variável de opções
    int number = 0;
    foreach (string opt in validOptions)
    {
        options.Add(number++, opt);
        Console.WriteLine($"({number}) {opt}");
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


// Função para validar a opção
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

// Função para pegar o nome do usuário
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

Welcome();
