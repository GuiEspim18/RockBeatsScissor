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

    string choiceName = playerOptions[choice];
    Console.WriteLine("Computador escolheu: " + pcOptions[pcChoice]);
}

int PcAvaliationOptions(Dictionary<int, string> playerOptions, Dictionary<int, string> pcOptions)
{
    // Regras do jogo: o que cada opção derrota
    var rules = new Dictionary<string, string>
    {
        { "Pedra", "Tesoura" },
        { "Papel", "Pedra" },
        { "Tesoura", "Papel" }
    };

    // Verificar se o computador tem uma opção que vença a do jogador
    foreach (var pcOption in pcOptions.Values)
    {
        foreach (var playerOption in playerOptions.Values)
        {
            if (rules[pcOption] == playerOption)
            {
                // Se o computador vencer, retorna a opção
                return pcOptions.FirstOrDefault(option => option.Value == pcOption).Key;
            }
        }
    }

    // Se não houver uma vitória clara, o computador escolhe aleatoriamente
    return pcOptions.Keys.ElementAt(new Random().Next(pcOptions.Count));
}

void ShowOptions(string[] options, bool pc = false)
{
    if (!pc)
    {
        Console.WriteLine("As suas opções são:");
    }
    else
    {
        Console.WriteLine("As minhas opções são:");
    }
    int hand = 1;
    foreach (string option in options)
    {
        Console.WriteLine($"Mão {hand} - {option}");
        hand++;
    }
}

void ChooseUser()
{
    Console.WriteLine("Com qual usuário você quer jogar ?");
    string[] keys = statitics.Keys.ToArray();
    int option = ChooseOptions(keys);
    user = keys[option - 1];
}

void Tutorial()
{
    Console.WriteLine("--Tutorial--");
    Console.WriteLine("Cada jogador usa as duas mãos, jogando duas vezes (uma com cada mão) e recebendo valores de pedra, papel ou tesoura. Depois, escolhe um dos valores para enfrentar o adversário. O vencedor é decidido pelas regras tradicionais: pedra vence tesoura, tesoura vence papel e papel vence pedra.");
}

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

void AddUser()
{
    Console.WriteLine("Digite o seu nome:");
    string name = GetName();
    while (statitics.ContainsKey(name))
    {
        Console.WriteLine("Este usuário já foi adicionado!");
        name = GetName();
    }
    statitics.Add(name, (0, 0, 0));
}

int ChooseOptions(string[] validOptions)
{
    Dictionary<int, string> options = new();
    int number = 0;
    foreach (string opt in validOptions)
    {
        options.Add(number++, opt);
        Console.WriteLine($"({number}) {opt}");
    }
    Console.WriteLine("Escolha uma opção:");
    string? option = Console.ReadKey().KeyChar.ToString();
    Console.WriteLine();
    bool valid = ValidateOpt(option, number);
    while (!valid)
    {
        Console.WriteLine("Opção inválida:");
        option = Console.ReadKey().KeyChar.ToString();
        Console.WriteLine();
        valid = ValidateOpt(option, number);
    }
    return int.Parse(option);

}

bool ValidateOpt(string option, int max)
{
    if (option != null)
    {
        int numOpt = int.Parse(option);
        if (numOpt > 0 && numOpt <= max)
        {
            return true;
        }
    }
    return false;
}

string GetName()
{
    Console.WriteLine("Qual o seu nome?");
    string? name = Console.ReadLine();
    Console.WriteLine();
    while (name == null)
    {
        Console.WriteLine("Seu nome não pode ser nulo!");
        name = Console.ReadLine();
        Console.WriteLine();
    }
    return name;
}

Welcome();
