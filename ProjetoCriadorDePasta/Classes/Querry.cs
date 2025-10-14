using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Security.AccessControl;
using System.Data;
namespace ProjetoCriadorDePasta.Classes
{
    public class Querry
    {
        public void CorrecoesBanco()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string update =
                        "UPDATE ESTSAL SET VRVENDAV = 0.01 WHERE VRVENDAV = 0" +
                        "UPDATE CADPRO SET ANPFISCALID = '' WHERE ANPFISCALID = 0" +
                        "update NCMFISCAL set descricao = 'migrado' where descricao =''";
                    SqlCommand command = new SqlCommand(update, cn)
                    { CommandTimeout = 600 };
                    command.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Correções feitas no banco, prosseguindo com a extração dos dados...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possivel realizar as correções no banco, Erro:" + ex.Message);
            }
        }

        public void SituacaoCliente(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @" 
                        SELECT
	                    SITUACAO,					-- Id
	                    '|',				   
	                    DESCRICAO,					-- Nome
	                    '|',				   
	                    CASE ESPECIAL WHEN 'S' THEN 1 ELSE 0 END,					-- Especial
	                    '|',				   
	                    CASE ATU_CADASTRO WHEN 'S' THEN 1 ELSE 0 END,					-- AtualizarCadastro
	                    '|',				   
	                    CASE EXIGE_SENHA WHEN 'S' THEN 1 ELSE 0 END,					-- ExigeSenha
	                    '|',				   
	                    CASE BLOQUEAR WHEN 'S' THEN 1 ELSE 0 END,					-- Bloquear
	                    '|',				   
	                    1					-- Ativo	
                    FROM CADSIT -- Tabela
                    ";
                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void TipoCliente(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @" 
                       SELECT
	                    CODIGO,					-- Id
	                    '|',				   
	                    DESCRICAO				-- Descricao				   
                    FROM TABELA 
                    WHERE TIPO = 'CADCLI.TIPO' 
                    ";
                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Usuario(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @" 
                       SELECT
	                    ID + 2,					-- Id
	                    '|',				   
	                    nome,					-- Nome
	                    '|',				   
	                    login,					-- Login
	                    '|',				   
	                    hora_entrada,					-- Hora Entrada
	                    '|',				   
	                    hora_saida,					-- Hora Saida
	                    '|',				   
	                    e_mail,					-- E-mail
	                    '|',				   
	                   case ativo when 'true' then 1 else 0 end					-- Ativo
                    FROM CADFUN-- Tabela
                    WHERE ID NOT IN ('999998','999999')
                    ";
                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        public void Cliente(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT distinct
	PESSOA.ID,
	'|',
	CASE WHEN PESSOA.TIPONATUREZA = 'F' THEN 1
	ELSE 2
	END,
	'|',
	CNPJCPF,
	'|',
	REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(pessoa.nome, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-',''),
	'|',
	CASE 
		WHEN LEN(PESSOA.PSEUDONIMO) < 2 THEN REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE( pessoa.nome, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')
		WHEN PESSOA.PSEUDONIMO IS NULL THEN REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(pessoa.nome, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')
		WHEN PESSOA.PSEUDONIMO = '' THEN REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(pessoa.nome, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')
	ELSE REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(pessoa.PSEUDONIMO, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')
	END,
	'|',
	CONVERT(CHAR(10), PESSOA.DATANASCIMENTO, 103) AS DATANASCIMENTO,
	'|',
	PESSOA.INSCRICAOESTADUAL,
	'|',
	PESSOA.INSCRICAOMUNICIPAL,
	'|',
	REPLACE(REPLACE(REPLACE(COALESCE(PESSOA.OBSERVACAOINTERNA, ''), CHAR(10), ''), CHAR(13), ''), '|', ''),
	'|',
	case 
	when PESSOA.INSCRICAOESTADUAL = 'ISENTO' then 2
	WHEN PESSOA.INSCRICAOESTADUAL = 'NULL' then 9
	when PESSOA.INSCRICAOESTADUAL = ''  then 9 
	when PESSOA.INSCRICAOESTADUAL <> '' and PESSOA.INSCRICAOESTADUAL <> 'ISENTO' then 1 else 9 end as contribuinte,		
	'|',
	0 AS PERMITEAPROVEITAMENTOCREDITO,
	'|',
case
    when PESSOA.ATIVO = 1 then '1'
    when pessoa.ativo = 0 then '0'
    else '0' end,
	'|',
	CASE 
		WHEN COALESCE(
				(SELECT FILIAL FROM CADCLI WHERE CADCLI.CLIENTE = PESSOA.ID),
				(SELECT TOP 1 ID FROM CADFIL WHERE ATIVO = 1 ORDER BY ID ASC)
			) = 1 THEN 1
		WHEN COALESCE(
				(SELECT FILIAL FROM CADCLI WHERE CADCLI.CLIENTE = PESSOA.ID),
				(SELECT TOP 1 ID FROM CADFIL WHERE ATIVO = 1 ORDER BY ID ASC)
			) = 50 THEN 2
		ELSE 99 -- valor padrão, caso não atenda nenhuma condição
	END AS filial,	
	'|',				   
	coalesce(cadcli.IDENTIDADE,''),	-- Identidade
	'|',				   
	coalesce(CADCLI.ORGAOIDENTIDADE,''), -- Orgao expeditor
	'|',					   
CASE CAST(COALESCE(UFIDENTIDADE, '') AS VARCHAR)
    WHEN 'RO' THEN '11'
    WHEN 'AC' THEN '12'
    WHEN 'AM' THEN '13'
    WHEN 'RR' THEN '14'
    WHEN 'PA' THEN '15'
    WHEN 'AP' THEN '16'
    WHEN 'TO' THEN '17'
    WHEN 'MA' THEN '21'
    WHEN 'PI' THEN '22'
    WHEN 'CE' THEN '23'
    WHEN 'RN' THEN '24'
    WHEN 'PB' THEN '25'
    WHEN 'PE' THEN '26'
    WHEN 'AL' THEN '27'
    WHEN 'SE' THEN '28'
    WHEN 'BA' THEN '29'
    WHEN 'MG' THEN '31'
    WHEN 'ES' THEN '32'
    WHEN 'RJ' THEN '33'
    WHEN 'SP' THEN '35'
    WHEN 'PR' THEN '41'
    WHEN 'SC' THEN '42'
    WHEN 'RS' THEN '43'
    WHEN 'MS' THEN '50'
    WHEN 'MT' THEN '51'
    WHEN 'GO' THEN '52'
    WHEN 'DF' THEN '53'
    WHEN 'EX' THEN '99'
    ELSE ''
END AS id_estado,	-- UF identidade
	'|',				   
	coalesce(CONVERT(VARCHAR(10), dtabertura, 103),''),	-- Data Abertura	
	'|',				   
	coalesce(pai,''),					-- Pai
	'|',				   
	coalesce(mae,''),					-- Mae
	'|',				   
	coalesce(natural,''),					-- Naturalidade	
	'|',				   
	CASE CAST(sexo AS varchar)
  WHEN 'M' THEN 1
  WHEN 'F' THEN 2
  WHEN 'O' THEN 3
  ELSE NULL
END,					-- Sexo
	'|',		   
	COALESCE(LIMITE_CREDITO,'0'),					-- Limite credito
	'|',				   
	COALESCE(LIMITE_DEBITO,'0'),					-- Limite debito	
	'|',				   
	COALESCE(vencimento,'0'),					-- Dias para vencimento
	'|',				   
	COALESCE(DESC_MATERIAL,'0'),				-- Desconto para produto
	'|',				   
	COALESCE(DESC_SERVICO,'0'),					-- Desconto para serviço	
	'|',				   
	COALESCE(vr_mensal,'0'),					-- Valor mensal
	'|',					   
	   CASE ESTADOCIVIL WHEN 'O' THEN 5
   WHEN 'S' THEN 1
   WHEN 'C' THEN 2
   WHEN 'P' THEN 5
   WHEN 'D' THEN 3
   WHEN 'V' THEN 4
   ELSE 5 END,					-- Estado civil
	'|',				   
	COALESCE(situacao,1),					-- Situacaoid	
	'|',				   
	coalesce(CONVERT(VARCHAR(10), dtsituacao, 103),''),					-- Datasituacao
	'|',				   
	'NULL',					-- VendedorID
	'|',				   
	COALESCE(tipo,''),					-- TipoclienteId	
	'|',				   
	COALESCE(grade,''),					-- ComplementoGrade
	'|'	,		
	REPLACE(
		REPLACE(
			REPLACE(
				REPLACE(
					REPLACE(
						COALESCE(CADCLI.ALERTA, '') COLLATE Latin1_General_CI_AI, 
					CHAR(13), ' '),
				CHAR(10), ' '),
			CHAR(9), ' '),
		'|', ''),
	NCHAR(160), ' ') AS alerta,
	'|'	,	
	case cast(tipovencimento as varchar) when '0' then '1'
	when '1' then '2'
	else '0' end as tipovencimento
FROM PESSOA
LEFT JOIN CONTRIBUINTE ON CONTRIBUINTE.ID = PESSOA.CONTRIBUINTEID
left join CADCLI on CADCLI.CLIENTE = PESSOA.ID
WHERE PESSOA.ID < 999999";

                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Cliente_Endereco(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                    SELECT distinct
	PESSOA.ID,	
	'|',
	CASE WHEN LEN(ENDERECO.LOGRADOURO) < 2 THEN 'LOGRADOURO NAO INFORMADO' ELSE REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(ENDERECO.LOGRADOURO, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')END,	
	'|',
	COALESCE(ENDERECO.NUMERO, '0'),
	'|',
	REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(ENDERECO.COMPLEMENTO, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-',''),
	'|',
	ENDERECO.CAIXAPOSTAL,
	'|',
	CASE
		WHEN ENDERECO.BAIRRO IS NULL THEN 'CENTRO' 
		WHEN LEN(ENDERECO.BAIRRO) < 2 THEN 'CENTRO'
		ELSE REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(ENDERECO.BAIRRO, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')
	END,
	'|',
	CASE 
		WHEN ENDERECO.CEP IS NULL THEN '29900000'
		WHEN LEN(ENDERECO.CEP) < 8 THEN LEFT(RTRIM(ENDERECO.CEP) + '00000000', 8) 
		ELSE ENDERECO.CEP END,
	'|',
	COALESCE(ENDERECO.MUNICIPIOID, '3203205'),
	'|',
	CASE 
     WHEN ENDERECO.ATIVO = 'true' THEN 1 
    ELSE 0 
     END AS ATIVO 
FROM ENDERECO
JOIN PESSOA ON PESSOA.ENDERECOID = ENDERECO.ID left join CADCLI on CADCLI.NOME = PESSOA.NOME
WHERE PESSOA.ID < 999999

ORDER BY PESSOA.ID ASC";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Cliente_Telefone(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT
	PESSOAID,
	'|',
	CASE PESSOATELEFONE.TIPOTELEFONE
		WHEN '1' THEN 1
		WHEN '2' THEN 2
		WHEN '9' THEN 3
		ELSE 3
	END,
	'|',
	TELEFONE,
	'|',
	1
FROM PESSOATELEFONE
WHERE PESSOAID < 999999 and telefone <> '' and tipotelefone <> '2' and telefone <> 'NULL'

UNION ALL

SELECT
	CLIENTE,
	'|',
	2,
	'|',
	celular,
	'|',
	1
FROM cadcli
where celular <> 'NULL' and celular <> ''
";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Cliente_Email(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                  SELECT distinct
	PESSOAID,
	'|',
	'0',
	'|',
	EMAIL,
	'|',
	case ATIVO when 'true' then 1 else 0 end as ativo
FROM PESSOAEMAIL
WHERE PESSOAID < 999999 and EMAIL <> 'NULL'
ORDER BY PESSOAID ASC
";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Cliente_DependenteConjuge(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                  SELECT
	                id,					-- ClienteId
	                '|',				   
	                2,					-- Tipo
	                '|',				   
	                REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
                COALESCE(nome, '')
                ,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-',''),					-- Nome
	                '|',				   
	                coalesce(CONVERT(VARCHAR(10), datanascimento, 103),''),					-- DataNascimento
	                '|',			   
	                ''					-- Observacao
                FROM PESSOACONJUGE-- Tabela
                where nome <> '' and nome is not null

                union all

                SELECT
	                pessoaid,					-- ClienteId
	                '|',				   
	                3,					-- Tipo
	                '|',				   
	                REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
                COALESCE(nome, '')
                ,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-',''),					-- Nome
	                '|',				   
	                coalesce(CONVERT(VARCHAR(10), datanascimento, 103),''),					-- DataNascimento
	                '|',				   
	                ''					-- Observacao
                FROM PESSOADEPENDENTE-- Tabela
                where pessoaid is not null and nome <> '' and nome is not null
                    ";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        /////////
        public void Fornecedor(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
SELECT
	PESSOA.ID - 1000000,
	'|',
	CASE WHEN PESSOA.TIPONATUREZA = 'F' THEN 1
	ELSE 2
	END,
	'|',
	PESSOA.CNPJCPF,
	'|',
		REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(pessoa.nome, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-',''),
	'|',
	CASE 
		WHEN LEN(PESSOA.PSEUDONIMO) < 2 THEN REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE( pessoa.nome, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')
		WHEN PESSOA.PSEUDONIMO IS NULL THEN REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(pessoa.nome, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')
		WHEN PESSOA.PSEUDONIMO = 'NULL' THEN REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(pessoa.nome, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')
		WHEN PESSOA.PSEUDONIMO = '' THEN REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(pessoa.nome, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')
	ELSE REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(pessoa.PSEUDONIMO, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')
	END,
	'|',
	CONVERT(CHAR(10), DATANASCIMENTO, 103) AS DATANASCIMENTO,
	'|',
	PESSOA.INSCRICAOESTADUAL,
	'|',
	PESSOA.INSCRICAOMUNICIPAL,
	'|',
	REPLACE(REPLACE(REPLACE(COALESCE(OBSERVACAOINTERNA, ''), CHAR(10), ''), CHAR(13), ''), '|', ''),
	'|',
	case when PESSOA.INSCRICAOESTADUAL <> '' and PESSOA.INSCRICAOESTADUAL <> 'ISENTO' then 1
	when PESSOA.INSCRICAOESTADUAL = 'ISENTO' then 2
	when PESSOA.INSCRICAOESTADUAL = '' then 9 else 9 end as contribuinte,	
	'|',
	CASE CADFOR.TIPO
		WHEN 'F' THEN 2
		WHEN 'S' THEN 1
		WHEN 'T' THEN 3
		WHEN 'I' THEN 4
		WHEN 'O' THEN 6
		ELSE 5
	END,
	'|',
	NULL AS HOMEPAGE,
	'|',
case
    when PESSOA.ATIVO = 1 then '1'
    when pessoa.ativo = 0 then '0'
    else '1' end
FROM PESSOA
LEFT JOIN CONTRIBUINTE ON CONTRIBUINTE.ID = PESSOA.CONTRIBUINTEID
JOIN CADFOR ON CADFOR.FORNECEDOR + 1000000 = PESSOA.ID 
WHERE PESSOA.ID > 1000000 AND PESSOA.ID <= 2000000";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Fornecedor_Endereco(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                  SELECT distinct
	PESSOA.ID - 1000000,	
	'|',
	CASE WHEN LEN(ENDERECO.LOGRADOURO) < 2 THEN 'LOGRADOURO NAO INFORMADO' ELSE REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(ENDERECO.LOGRADOURO, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')END,
	'|',
	COALESCE(ENDERECO.NUMERO, '0'),
	'|',
		REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(ENDERECO.COMPLEMENTO, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-',''),
	'|',
	ENDERECO.CAIXAPOSTAL,
	'|',
	CASE
		WHEN ENDERECO.BAIRRO IS NULL THEN 'CENTRO' 
		WHEN LEN(ENDERECO.BAIRRO) < 2 THEN 'CENTRO'
		ELSE REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(ENDERECO.BAIRRO, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-','')
	END,
	'|',
	CASE 
		WHEN ENDERECO.CEP IS NULL THEN '29900000'
		WHEN LEN(ENDERECO.CEP) < 8 THEN LEFT(RTRIM(ENDERECO.CEP) + '00000000', 8) 
		ELSE ENDERECO.CEP END,
	'|',
	COALESCE(ENDERECO.MUNICIPIOID, '3203205'),
	'|',
	case ENDERECO.ATIVO when 'TRUE' then 1 else 0 end
FROM ENDERECO
JOIN PESSOA ON PESSOA.ENDERECOID = ENDERECO.ID
WHERE PESSOA.ID >= 1000000 AND PESSOA.ID < 2000000";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Fornecedor_Telefone(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT
	PESSOAID - 1000000,
	'|',
	CASE PESSOATELEFONE.TIPOTELEFONE
		WHEN '1' THEN 1
		WHEN '2' THEN 2
		WHEN '9' THEN 3
		ELSE 3
	END,
	'|',
	TELEFONE,
	'|',
	ATIVO
FROM PESSOATELEFONE
WHERE PESSOAID >= 1000000 AND PESSOAID < 2000000 and telefone <> 'NULL' and telefone <> ''

union all 

SELECT
	fornecedor,
	'|',
	2,
	'|',
	contato_celular,
	'|',
	1
FROM cadfor
where contato_celular <> 'NULL' and contato_celular <> ''


";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Fornecedor_Email(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                  SELECT
	PESSOAID - 1000000,
	'|',
	'0',
	'|',
	EMAIL,
	'|',
	case ATIVO when 'TRUE' then 1 else 0 end
FROM PESSOAEMAIL
WHERE PESSOAID >= 1000000 AND PESSOAID < 2000000
ORDER BY PESSOAID ASC";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Fornecedor_Contato(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT
	FORNECEDOR,
	'|',
	CONTATO,
	'|',
	CONTATO_FONE,
	'|',
	CONTATO_E_MAIL,
	'|',
	1
FROM CADFOR
JOIN PESSOA ON PESSOA.ID = (CADFOR.FORNECEDOR + 1000000)
WHERE CONTATO <> '' AND CONTATO IS NOT NULL
";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        //////////
        public void cest(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                   SELECT
	CESTFISCAL.ID,
	'|',
	CESTFISCAL.NOME,
	'|',
	case CESTFISCAL.ATIVO when 'TRUE' then 1 else 0 end
FROM CESTFISCAL";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void ncm(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                   SELECT
	NCMFISCAL.ID,
	'|',
	NCMFISCAL.NOME,
	'|',
	case NCMFISCAL.ATIVO when 'TRUE' then 1 else 0 end 
FROM NCMFISCAL where LEN(NCMFISCAL.ID) = 8";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void cestxncm(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                   SELECT
	CESTFISCAL_NCMFISCAL.CESTFISCALID,
	'|',
	CESTFISCAL_NCMFISCAL.NCMFISCALID,
	'|',
	case CESTFISCAL_NCMFISCAL.ATIVO when 'TRUE' then 1 else 0 end
FROM CESTFISCAL_NCMFISCAL";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void markup(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                   SELECT
	ROW_NUMBER() OVER(ORDER BY DATA) + 1, /* Markup 1 ja existe no banco da web. O script 11.1 PRECOS PRODUTOS utiliza a mesma lógica desse arquivo para configurar o Markup */
	'|',
	CADMKP.NOME,
	'|',
	CADMKP.PERCENTUAL,
	'|',
	case CADMKP.ATIVO when 'true' then 1 else 0 end
FROM CADMKP";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Unidade(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                   SELECT 
	  UNIDADEPRODUTO.ID,
	  '|',
	  UNIDADEPRODUTO.NOME,
	  '|',
	 case ATIVO when 'TRUE' then 1 else 0 end
FROM UNIDADEPRODUTO

";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Marca(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                   SELECT 
	  FORMAR.ID,
	  '|',
	  FORMAR.DESCRICAO,
	  '|',
	  case FORMAR.ATIVO when 'true' then 1 else 0 end
FROM FORMAR
ORDER BY ID ASC";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Grupo(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"----- ====== CRIA GRUPOS PADROES ====== ----
IF NOT EXISTS(SELECT [dbo].[ESTGRU].[ID] FROM [dbo].[ESTGRU] WHERE [dbo].[ESTGRU].[ID] = '99')
BEGIN
	INSERT INTO [dbo].[ESTGRU]
			   ([ID], [DESCRICAO], [COM_AVISTA], [COM_ENTRADA], [COM_APRAZO]
			   ,[COM_CHEQUE], [COM_CHEQUEPRE], [COM_CARTAO], [COM_ELECTRON], [COM_OUTROS]
			   ,[COM_FINANCEIRA], [LUCRO], [ADMITE_MOV], [DATA], [ATIVO], [EXPORTADO])
		 VALUES ('99','DEPARTAMENTO DIVERSOS'
			   ,0, 0, 0, 0, 0, 0, 0, 0, 0, 0,'S', GETDATE(), 1, 1)
END

IF NOT EXISTS(SELECT [dbo].[ESTGRU].[ID] FROM [dbo].[ESTGRU] WHERE [dbo].[ESTGRU].[ID] = '9999')
BEGIN
	INSERT INTO [dbo].[ESTGRU]
			   ([ID], [DESCRICAO], [COM_AVISTA], [COM_ENTRADA], [COM_APRAZO]
			   ,[COM_CHEQUE], [COM_CHEQUEPRE], [COM_CARTAO], [COM_ELECTRON], [COM_OUTROS]
			   ,[COM_FINANCEIRA], [LUCRO], [ADMITE_MOV], [DATA], [ATIVO], [EXPORTADO])
		 VALUES ('9999','SETOR DIVERSOS'
			   ,0, 0, 0, 0, 0, 0, 0, 0, 0, 0,'S', GETDATE(), 1, 1)
END

IF NOT EXISTS(SELECT [dbo].[ESTGRU].[ID] FROM [dbo].[ESTGRU] WHERE [dbo].[ESTGRU].[ID] = '9999999')
BEGIN
	INSERT INTO [dbo].[ESTGRU]
			   ([ID], [DESCRICAO], [COM_AVISTA], [COM_ENTRADA], [COM_APRAZO]
			   ,[COM_CHEQUE], [COM_CHEQUEPRE], [COM_CARTAO], [COM_ELECTRON], [COM_OUTROS]
			   ,[COM_FINANCEIRA], [LUCRO], [ADMITE_MOV], [DATA], [ATIVO], [EXPORTADO])
		 VALUES ('9999999','SECAO DIVERSOS'
			   ,0, 0, 0, 0, 0, 0, 0, 0, 0, 0,'S', GETDATE(), 1, 1)
END

----- ====== DECLARACOES DE VARIAVEIS ====== ----

DECLARE @TABELASEQUENCIA TABLE(
	SEQ INT,
	DEPARTAMENTO VARCHAR(100),
	SETOR VARCHAR(100),
	SECAO VARCHAR(100),
	ID VARCHAR(100)
)

DECLARE @TABELAFINALGRUPO TABLE(
	SEQ INT,
	DESCRICAO VARCHAR(100),
	CODRELACIONAMENTO VARCHAR(100),
	HIERARQUIA VARCHAR(10)
)

----- ====== INSERT TABELA AUXILIAR DE SEQUENCIA DO GRUPO ====== ----

INSERT INTO @TABELASEQUENCIA (SEQ, DEPARTAMENTO, SETOR, SECAO, ID)
	SELECT
		ROW_NUMBER() OVER (ORDER BY LEN(ESTGRUSQ.ID), ESTGRUSQ.ID) + 3 AS SEQ /* +3 == GRUPOS PADRAO DO SISTEMA */
	   ,SUBSTRING(ESTGRUSQ.ID, 1, 2) AS DEPARTAMENTO
	   ,SUBSTRING(ESTGRUSQ.ID, 3, 2) AS SETOR
	   ,SUBSTRING(ESTGRUSQ.ID, 5, 3) AS SECAO
	   ,ESTGRUSQ.ID AS ID
	FROM ESTGRU AS ESTGRUSQ
	WHERE ESTGRUSQ.ID NOT IN('99', '9999', '9999999')

----- ====== INSERT NA TABELA FINAL DE GRUPO ====== ----

INSERT INTO @TABELAFINALGRUPO(SEQ, DESCRICAO, CODRELACIONAMENTO, HIERARQUIA)
	SELECT 1, ESTGRU.DESCRICAO, NULL, 1 FROM ESTGRU WHERE ESTGRU.ID = '99'
	UNION ALL
	SELECT 2, ESTGRU.DESCRICAO, 1, 2 FROM ESTGRU WHERE ESTGRU.ID = '9999'
	UNION ALL
	SELECT 3, ESTGRU.DESCRICAO, 2, 3 FROM ESTGRU WHERE ESTGRU.ID = '9999999'

INSERT INTO @TABELAFINALGRUPO(SEQ, DESCRICAO, CODRELACIONAMENTO, HIERARQUIA)
SELECT
	TABELASEQUENCIA.SEQ
   ,ESTGRU.DESCRICAO
   ,CASE LEN(ESTGRU.ID)
		WHEN 2 THEN NULL
		WHEN 4 THEN COALESCE(CAST((SELECT
					TABELASEQUENCIASQ.SEQ
				FROM @TABELASEQUENCIA AS TABELASEQUENCIASQ
				WHERE TABELASEQUENCIASQ.DEPARTAMENTO = TABELASEQUENCIA.DEPARTAMENTO
				AND REPLACE(TABELASEQUENCIASQ.SETOR, ' ', '') = ''
				AND REPLACE(TABELASEQUENCIASQ.SECAO, ' ', '') = '')
			AS VARCHAR(100)), 1)  /* 2 == DEPARTAMENTO DIVERSOS */

		ELSE COALESCE((SELECT
					TABELASEQUENCIASQ.SEQ
				FROM @TABELASEQUENCIA AS TABELASEQUENCIASQ
				WHERE TABELASEQUENCIASQ.DEPARTAMENTO = TABELASEQUENCIA.DEPARTAMENTO
				AND TABELASEQUENCIASQ.SETOR = TABELASEQUENCIA.SETOR
				AND REPLACE(TABELASEQUENCIASQ.SECAO, ' ', '') = '')
			, COALESCE((SELECT
					TABELASEQUENCIASQ.SEQ
				FROM @TABELASEQUENCIA AS TABELASEQUENCIASQ
				WHERE TABELASEQUENCIASQ.DEPARTAMENTO = TABELASEQUENCIA.DEPARTAMENTO
				AND REPLACE(TABELASEQUENCIASQ.SETOR, ' ', '') = ''
				AND REPLACE(TABELASEQUENCIASQ.SECAO, ' ', '') = ''), (2)) /* 2 == SETOR DIVERSOS */
			)
	END AS RELOBJWEBDB,
	CASE
		WHEN LEN(ESTGRU.ID) <= 2 THEN 1
		WHEN LEN(ESTGRU.ID) >= 3 AND LEN(ESTGRU.ID) <= 4 THEN 2
		WHEN LEN(ESTGRU.ID) >= 5 AND LEN(ESTGRU.ID) <= 7 THEN 3
	END
FROM ESTGRU
LEFT JOIN @TABELASEQUENCIA AS TABELASEQUENCIA
	ON TABELASEQUENCIA.ID = ESTGRU.ID
WHERE ESTGRU.ID NOT IN('99', '9999', '9999999')
ORDER BY LEN(ESTGRU.ID), ESTGRU.ID

--select * FROM @TABELAFINALGRUPO order by descricao
--select * From ESTGRU order by descricao
SELECT 
	SEQ,
	'|',
	REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(DESCRICAO, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-',''),
	'|',
	CODRELACIONAMENTO,
	'|',
	HIERARQUIA,
	'|',
	1
FROM @TABELAFINALGRUPO
ORDER BY SEQ ASC";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Grades(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT 
	LTRIM(RTRIM(GRADE)),
	'|',
	1
FROM ESTGRA
WHERE LEN(LTRIM(RTRIM(GRADE))) > 0
GROUP BY LTRIM(RTRIM(GRADE))
ORDER BY COUNT(*) DESC
";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Localizacao(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                   SELECT
	                    id					-- NOME
                    FROM localizacaoproduto";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Montadora(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"
                   SELECT
	                id,					-- Id
	                '|',				   
	                descricao,					-- Nome
	                '|',				   
	                case ativo when 'true' then 1 else 0 end					-- ativo
                FROM cadmon -- Tabela";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Produto(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT
	CADPRO.ID,
	'|',
	REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
COALESCE(cadpro.descricao, '')
,'Ç','C'),'Ñ','N'),'Ý','Y'),'Á','A'),'À','A'),'Â','A'),'Ã','A'),'Ä','A'),'É','E'),'È','E'),'Ê','E'),'Ë','E'),'Í','I'),'Ì','I'),'Î','I'),'Ï','I'),'Ó','O'),'Ò','O'),'Ô','O'),'Õ','O'),'Ö','O'),'Ú','U'),'Ù','U'),'Û','U'),'Ü','U'),'–',''),'-',''),
	'|',
	CADPRO.REFERENCIA,
	'|',
	CADPRO.CODIGO_BARRA,
	'|',
	CADPRO.PESOV,
	'|',
	CADPRO.VOLUME,
	'|',
	CASE WHEN CSTICMSID = '060' THEN '0500'
		 WHEN CSTICMSID = '000' THEN '0102'
		 ELSE '0102'
	END,
	'|',
	0 AS MONOFASICO,
	'|',
	0 AS COMBUSTIVEL,
	'|',
	CASE CADPRO.ALTERAR_VALOR
		WHEN 'S' THEN 1
		ELSE '0'
	END AS ALTERAR_VALOR,
	'|',
	CASE CADPRO.UTILIZABALANCA
		WHEN 'S' THEN 1
		ELSE '0'
	END AS PESAVEL,
	'|',
	CADPRO.UNIDADEPRODUTOID,
	'|',
	--CADPRO.CODIGONCM,
	CASE WHEN LEN(LTRIM(RTRIM(COALESCE(CADPRO.CODIGONCM, '')))) > 0
		THEN CADPRO.CODIGONCM
		ELSE '12040090' --0025654582
	END AS NCMFISCALID, /* Em situacoes onde nao existe o NCMFISCALID Relacionado, utilizar esse codigo */
	'|',
	CADPRO.CESTFISCALID,
	'|',
	CADPRO.ANPFISCALID,
	'|',	
	ESTGRU.DESCRICAO,
	'|',
	CADPRO.MARCA,
	'|',
	CADPRO.GENEROFISCALID,
	'|',
	CADPRO.EXTIPIFISCALID,
	'|',
case
    when CADPRO.ATIVO = 1 then '1'
    when CADPRO.ATIVO = 0 then '0'
    else '0' end,
	'|',
	CASE CADPRO.EDITAR
		WHEN 'S' THEN 1
		ELSE '0'
	END AS ALTERADESCRICAO,			--alteradescricaosaida,
	'|',
	CADPRO.aplicacao,			--aplicacao,
	'|',
case 
	when (SELECT MAX(PROMOCAOQTDV) FROM ESTSAL WHERE ESTSAL.MATRICULA = CADPRO.ID  and filial = 1) is null then 0.0000
	else (SELECT MAX(PROMOCAOQTDV) FROM ESTSAL WHERE ESTSAL.MATRICULA = CADPRO.ID  and filial = 1) end as quantidadepromocao,			--quantidadepromocao
	'|',
	CADPRO.DESCRICAO_NF,			--descricaofiscal
	'|',
	CASE cadpro.tipoitem
    WHEN 0 THEN '0'
    WHEN 1 THEN '1'
    WHEN 4 THEN '4'
    WHEN 7 THEN '7'
    WHEN 8 THEN '8'
    WHEN 99 THEN '99'
    ELSE '0'
END AS TipoItemFiscalDescricao,			--tipoitemfiscal
	'|',
case 
	when (select max(fator_basev) from estsal  WHERE ESTSAL.MATRICULA = CADPRO.ID and filial = 1) is null then 0.0000
	else (select max(fator_basev) from estsal  WHERE ESTSAL.MATRICULA = CADPRO.ID and filial = 1) end as fatorpreco,			--quantidadepromocao
	'|',
case 
	when (select max(fator_baseqtdv) from estsal  WHERE ESTSAL.MATRICULA = CADPRO.ID and filial = 1) is null then 0.0000
	else (select max(fator_baseqtdv) from estsal  WHERE ESTSAL.MATRICULA = CADPRO.ID and filial = 1) end as fatorpreco			--fatorquantidade	
FROM CADPRO
JOIN ESTGRU ON ESTGRU.ID = CADPRO.GRUPO
WHERE CADPRO.ID > 1";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Saldo(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT 
	CADPRO.ID,
	'|',
CASE
	WHEN ESTSAL.FILIAL = 50 THEN 2
	ELSE ESTSAL.FILIAL END,
	'|',
	1 AS TABELAPRECO,
	'|',
	NULL, --TABELAMARKUP.CODIGOMARKUP AS MARKUP,
	'|',
	COALESCE(ESTSAL.VRVENDAV, 0),
	'|',
	COALESCE(ESTSAL.VRCOMPRAV, 0),
	'|',
	COALESCE(ESTSAL.VRCUSTOV, 0),
	'|',
	COALESCE(ESTSAL.VRZEROV, 0),
	'|',
	COALESCE(ESTSAL.VRMEDIOV, 0),
	'|',
	COALESCE(ESTSAL.LUCROV, 0),
	'|',
	case ESTSAL.ATIVO when 'true' then 1 else 0 end,
    '|',
CASE
	WHEN estsal.descontov IS NULL THEN 0
	ELSE estsal.descontov END
 FROM CADPRO
 JOIN ESTSAL ON ESTSAL.MATRICULA = CADPRO.ID
 LEFT JOIN (
	SELECT
	ROW_NUMBER() OVER(ORDER BY DATA) + 1 AS CODIGOMARKUP /* Markup 1 ja existe no banco da web */, CADMKP.ID, CADMKP.NOME, CADMKP.PERCENTUAL, CADMKP.ATIVO FROM CADMKP
 ) AS TABELAMARKUP ON TABELAMARKUP.ID = ESTSAL.IDCADMKP
 WHERE CADPRO.ID > 1
 ORDER BY CADPRO.ID ASC
";

                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void GradeProduto(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT 
	CADPRO.ID AS PRODUTOID, 
	'|',
CASE 
	WHEN ESTSAL.FILIAL = 50 THEN 2
	ELSE ESTSAL.FILIAL END AS FILIALID,
	'|',
	CASE
		WHEN LEN(LTRIM(RTRIM(GRADE))) > 0
			THEN LTRIM(RTRIM(GRADE))
		ELSE 'U'
	END AS GRADE,
	'|',
	ESTGRA.ESTOQUEFISICO AS QUANTIDADE,
	'|',
	case ESTGRA.ATIVO when 'true' then 1 else 0 end,
    '|',
CASE
	WHEN estsal.ESTOQUE_MINIMOV IS NULL THEN 0
	ELSE  estsal.ESTOQUE_MINIMOV END
FROM CADPRO
JOIN ESTSAL ON ESTSAL.MATRICULA = CADPRO.ID
JOIN ESTGRA ON ESTGRA.IDESTSAL = ESTSAL.ID
WHERE CADPRO.ID > 1
ORDER BY PRODUTOID ASC
";

                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void CodigoBarra(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT 
	ESTREL.ID,
	'|',
	ESTREL.MATRICULA,
	'|',
	CASE
		WHEN LEN(LTRIM(RTRIM(GRADE))) > 0
			THEN LTRIM(RTRIM(GRADE))
		ELSE 'U'
	END AS GRADE,
	'|',
	case ESTREL.ATIVO when 'true' then 1 else 0 end
FROM ESTREL
";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void CodigoTerceiro(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT
	CODIGOTERCEIROPRODUTO.CADPROID,
	'|',
	CODIGOTERCEIROPRODUTO.CODIGOTERCEIRO,
	'|',
	CODIGOTERCEIROPRODUTO.CNPJCPF,	
	'|',
	case CODIGOTERCEIROPRODUTO.ATIVO when 'true' then 1 else 0 end
FROM CODIGOTERCEIROPRODUTO
WHERE CODIGOTERCEIROPRODUTO.ATIVO = 1
AND CODIGOTERCEIROPRODUTO.CNPJCPF IN(SELECT CADFOR.CNPJ FROM CADFOR) /* Acrescentar essa clausula caso queira ignorar registros em que o CNPJ do Fornecedor nao existe na CADFOR */
ORDER BY CODIGOTERCEIROPRODUTO.CADPROID
";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void ProdutoMontadora(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT
	                matricula,					-- ProdutoId
	                '|',				   
	                montadora					-- MontadoraId
                    FROM estmon-- Tabela
                    ";

                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void ProdutoSimilar(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT
	                    matricula,					-- ProdutoId
	                    '|',				   
	                    similar					-- Similarid
                    FROM estsim -- Tabela
                    ";

                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void ProdutoLocalizacao(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT
	                matricula,					-- ProdutoId
	                '|',				   
	                LOCALIZACAOPRODUTOid 					-- Nome Localizacao
                from ESTSAL_LOCALIZACAOPRODUTO -- Tabela
                left join estsal on estsal.id = ESTSAL_LOCALIZACAOPRODUTO.estsalid
                    ";

                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Clidoc(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT
	CAST(CLIDOC.FILIAL AS VARCHAR(100)),
	'| ',		-- Codigo da Filial
	CASE WHEN CADPOR.DEBITO = 'N' AND CADPOR.CREDITO = 'S' THEN '2' ELSE '1' END,			
	'| ',		-- Tipo do Documento 1 - Receber e 2 - Pagar
	CAST(CASE CLIDOC.CLIENTE WHEN 999999 THEN 406 ELSE COALESCE(CLIDOC.CLIENTE, 1) END AS VARCHAR(100)),			
	'| ',		-- Codigo do Cliente
	COALESCE(CLIDOC.CONTRATO, 0),		
	'| ',		-- Numero do Documento
	COALESCE(CLIDOC.PARCELA, ''),		
	'| ',		-- Numero da Parcela
	CONVERT(VARCHAR(100), CLIDOC.DT_EMISSAO, 103),			
	'| ',		-- Data de Emissao
	CONVERT(VARCHAR(100), CLIDOC.DT_VENCIMENTO, 103),			
	'| ',		-- Data de Vencimento
	CONVERT(VARCHAR(100), CLIDOC.DT_PAGAMENTO, 103),		
	'| ',		-- Data de Baixa
	CONVERT(VARCHAR(100), CLIDOC.DT_PRORROGACAO, 103),			
	'| ',		-- Data de Prorrogacao
	CAST(COALESCE(CLIDOC.VR_PARCELA, 0) AS VARCHAR(100)),			
	'| ',		-- Valor da Parcela
	COALESCE(CLIDOC.VR_JUROS, 0),			
	'| ',		-- Valor de Juros
	COALESCE(CLIDOC.VR_MULTA, 0),			
	'| ',		-- Valor de Multa
	COALESCE(CLIDOC.VR_DESCONTO, 0),			
	'| ',		-- Valor de Desconto
	COALESCE(VR_PAGO, 0) + COALESCE(VR_JUROS, 0) + COALESCE(VR_MULTA, 0) - COALESCE(VR_DESCONTO, 0),			
	'| ',		-- Valor Baixado
	COALESCE(CAST(CLIDOC.NOTA_FISCAL AS VARCHAR(100)), 'NULL'),		
	'| ',		-- Numero da Nota Fiscal ou Cupom Fiscal
	COALESCE(CAST(CLIDOC.FATURA AS VARCHAR(20)), ''),			
	'| ',		-- Numero da Fatura
	CAST(COALESCE(CLIDOC.DUPLICATA, '') AS VARCHAR(20)),			
	'| ',		-- Numero da Duplicat
	REPLACE(REPLACE(COALESCE(CLIDOC.TEXTO, ''), CHAR(13), ''), CHAR(10), ''),			
	'| ',		-- Observacoes
	case CLIDOC.ATIVO when 'true' then 1 else 0 end
				-- Ativo

FROM CLIDOC 
LEFT JOIN CADPOR ON CADPOR.PORTADOR = CLIDOC.PORTADOR
WHERE CLIDOC.VR_PARCELA > COALESCE(CLIDOC.VR_PAGO, 0) AND CLIDOC.ATIVO = 1
";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Fordoc(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT
CASE
	when CAST(FORDOC.FILIAL AS VARCHAR(100)) = '50' then '2'
	else CAST(FORDOC.FILIAL AS VARCHAR(100)) end,
	'| ',		-- Codigo da Filial
	CASE WHEN CADPOR.DEBITO = 'N' AND CADPOR.CREDITO = 'S' THEN '1' ELSE '2' END,			
	'| ',		-- Tipo do Documento 1 - Receber e 2 - Pagar
	FORDOC.FORNECEDOR ,			
	'| ',		-- Codigo do Cliente
	COALESCE(FORDOC.DOCUMENTO, 0),		
	'| ',		-- Numero do Documento
	COALESCE(FORDOC.PARCELA, ''),		
	'| ',		-- Numero da Parcela
	CONVERT(VARCHAR(100), FORDOC.DT_EMISSAO, 103),			
	'| ',		-- Data de Emissao	
	CONVERT(VARCHAR(100),
CASE
	when FORDOC.DT_VENCIMENTO < FORDOC.DT_EMISSAO then FORDOC.DT_EMISSAO
	else FORDOC.DT_VENCIMENTO
		
	end, 103) as DT_VENCIMENTO,			
	'| ',		-- Data de Vencimento
	CONVERT(VARCHAR(100), FORDOC.DT_PAGAMENTO, 103),		
	'| ',		-- Data de Baixa
	CONVERT(VARCHAR(100), FORDOC.DT_PRORROGACAO, 103),			
	'| ',		-- Data de Prorrogacao
	CAST(COALESCE(FORDOC.VR_PARCELA, 0) AS VARCHAR(100)),			
	'| ',		-- Valor da Parcela
	COALESCE(FORDOC.VR_JUROS, 0),			
	'| ',		-- Valor de Juros
	COALESCE(FORDOC.VR_MULTA, 0),			
	'| ',		-- Valor de Multa
	COALESCE(FORDOC.VR_DESCONTO, 0),			
	'| ',		-- Valor de Desconto
	COALESCE(VR_PAGO, 0) + COALESCE(VR_JUROS, 0) + COALESCE(VR_MULTA, 0) - COALESCE(VR_DESCONTO, 0),			
	'| ',		-- Valor Baixado
	COALESCE(CAST(FORDOC.NOTA_FISCAL AS VARCHAR(100)), 'NULL'),		
	'| ',		-- Numero da Nota Fiscal ou Cupom Fiscal
	'',			
	'| ',		-- Numero da Fatura
	CAST(COALESCE(FORDOC.DUPLICATA, '') AS VARCHAR(20)),			
	'| ',		-- Numero da Duplicat
	REPLACE(REPLACE(
		COALESCE(FORDOC.TEXTO, '') +
		CASE
			when
				FORDOC.DT_VENCIMENTO < FORDOC.DT_EMISSAO
				then  ' [VENCIMENTO AJUSTADO DE ' + CONVERT(VARCHAR(10), FORDOC.DT_VENCIMENTO, 103) + ' PARA ' + CONVERT(VARCHAR(10), FORDOC.DT_EMISSAO, 103) + ']'
				else ''
		end,
		CHAR(13), ''), CHAR(10), ''),		
	'| ',		-- Observacoes
	CASE COALESCE(FORDOC.USU_EXC, 0) WHEN 0 THEN 1 ELSE 0 END
				-- Ativo

FROM FORDOC 
LEFT JOIN CADPOR ON CADPOR.PORTADOR = FORDOC.PORTADOR
WHERE FORDOC.VR_PARCELA > COALESCE(FORDOC.VR_PAGO, 0) AND CASE COALESCE(FORDOC.USU_EXC, 0) WHEN 0 THEN 1 ELSE 0 END = 1
";

                    SqlCommand command = new SqlCommand(query, cn);
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        //Saldo zerado
        public void GradeProdutoZerado(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT 
	CADPRO.ID AS PRODUTOID, 
	'|',
CASE 
	WHEN ESTSAL.FILIAL = 50 THEN 2
	ELSE ESTSAL.FILIAL END AS FILIALID,
	'|',
	CASE
		WHEN LEN(LTRIM(RTRIM(GRADE))) > 0
			THEN LTRIM(RTRIM(GRADE))
		ELSE 'U'
	END AS GRADE,
	'|',
	0 AS QUANTIDADE,
	'|',
	case ESTGRA.ATIVO when 'true' then 1 else 0 end
FROM CADPRO
JOIN ESTSAL ON ESTSAL.MATRICULA = CADPRO.ID
JOIN ESTGRA ON ESTGRA.IDESTSAL = ESTSAL.ID
WHERE CADPRO.ID > 1
ORDER BY PRODUTOID ASC
";

                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }


        //Metodos para os filtros de filial
        public void Saldo(string LocalDiretorio, string filial)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT 
	CADPRO.ID,
	'|',
	1,
	'|',
	1 AS TABELAPRECO,
	'|',
	NULL, --TABELAMARKUP.CODIGOMARKUP AS MARKUP,
	'|',
	COALESCE(ESTSAL.VRVENDAV, 0),
	'|',
	COALESCE(ESTSAL.VRCOMPRAV, 0),
	'|',
	COALESCE(ESTSAL.VRCUSTOV, 0),
	'|',
	COALESCE(ESTSAL.VRZEROV, 0),
	'|',
	COALESCE(ESTSAL.VRMEDIOV, 0),
	'|',
	COALESCE(ESTSAL.LUCROV, 0),
	'|',
	case ESTSAL.ATIVO when 'true' then 1 else 0 end
 FROM CADPRO
 JOIN ESTSAL ON ESTSAL.MATRICULA = CADPRO.ID
 LEFT JOIN (
	SELECT
	ROW_NUMBER() OVER(ORDER BY DATA) + 1 AS CODIGOMARKUP /* Markup 1 ja existe no banco da web */, CADMKP.ID, CADMKP.NOME, CADMKP.PERCENTUAL, CADMKP.ATIVO FROM CADMKP
 ) AS TABELAMARKUP ON TABELAMARKUP.ID = ESTSAL.IDCADMKP
 WHERE CADPRO.ID > 1 and estsal.filial = @filial
 ORDER BY CADPRO.ID ASC
";

                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    command.Parameters.Add(new SqlParameter("@filial", SqlDbType.NVarChar) { Value = filial });
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void GradeProduto(string LocalDiretorio, string filial)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT 
	CADPRO.ID AS PRODUTOID, 
	'|',
CASE 
	WHEN ESTSAL.FILIAL = 50 THEN 2
	ELSE ESTSAL.FILIAL END AS FILIALID,
	'|',
	CASE
		WHEN LEN(LTRIM(RTRIM(GRADE))) > 0
			THEN LTRIM(RTRIM(GRADE))
		ELSE 'U'
	END AS GRADE,
	'|',
	ESTGRA.ESTOQUEFISICO AS QUANTIDADE,
	'|',
	case ESTGRA.ATIVO when 'true' then 1 else 0 end
FROM CADPRO
JOIN ESTSAL ON ESTSAL.MATRICULA = CADPRO.ID
JOIN ESTGRA ON ESTGRA.IDESTSAL = ESTSAL.ID
WHERE CADPRO.ID > 1 and estsal.filial = @filial
ORDER BY PRODUTOID ASC
";

                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    command.Parameters.Add(new SqlParameter("@filial", SqlDbType.NVarChar) { Value = filial });
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Clidoc(string LocalDiretorio, string filial)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT
	CASE 
		WHEN CLIDOC.FILIAL = 1 THEN 1
		WHEN CLIDOC.FILIAL = 2 THEN 2
		WHEN CLIDOC.FILIAL = 3 THEN 3
		WHEN CLIDOC.FILIAL = 4 THEN 4
		WHEN CLIDOC.FILIAL = 10 THEN 5
		WHEN CLIDOC.FILIAL = 11 THEN 6
		WHEN CLIDOC.FILIAL = 12 THEN 7
		WHEN CLIDOC.FILIAL = 14 THEN 8
		WHEN CLIDOC.FILIAL = 51 THEN 9
		WHEN CLIDOC.FILIAL = 52 THEN 10
		WHEN CLIDOC.FILIAL = 53 THEN 11
		WHEN CLIDOC.FILIAL = 54 THEN 12
		WHEN CLIDOC.FILIAL = 50 THEN 2
		ELSE 99 -- valor padrão
	END AS filial_tratada,
	'| ',		-- Codigo da Filial
	CASE WHEN CADPOR.DEBITO = 'N' AND CADPOR.CREDITO = 'S' THEN '2' ELSE '1' END,			
	'| ',		-- Tipo do Documento 1 - Receber e 2 - Pagar
	CAST(CASE CLIDOC.CLIENTE WHEN 999999 THEN 1 ELSE COALESCE(CLIDOC.CLIENTE, 1) END AS VARCHAR(100)),			
	'| ',		-- Codigo do Cliente
	COALESCE(CLIDOC.CONTRATO, 0),		
	'| ',		-- Numero do Documento
	COALESCE(CLIDOC.PARCELA, ''),		
	'| ',		-- Numero da Parcela
	CONVERT(VARCHAR(100), CLIDOC.DT_EMISSAO, 103),			
	'| ',		-- Data de Emissao
	CONVERT(VARCHAR(100),
CASE
	when CLIDOC.DT_VENCIMENTO < CLIDOC.DT_EMISSAO then CLIDOC.DT_EMISSAO
	else CLIDOC.DT_VENCIMENTO
		
	end, 103) as DT_VENCIMENTO,			
	'| ',		-- Data de Vencimento
	CONVERT(VARCHAR(100), CLIDOC.DT_PAGAMENTO, 103),		
	'| ',		-- Data de Baixa
	CONVERT(VARCHAR(100), CLIDOC.DT_PRORROGACAO, 103),			
	'| ',		-- Data de Prorrogacao
	CAST(COALESCE(CLIDOC.VR_PARCELA, 0) AS VARCHAR(100)),			
	'| ',		-- Valor da Parcela
	COALESCE(CLIDOC.VR_JUROS, 0),			
	'| ',		-- Valor de Juros
	COALESCE(CLIDOC.VR_MULTA, 0),			
	'| ',		-- Valor de Multa
	COALESCE(CLIDOC.VR_DESCONTO, 0),			
	'| ',		-- Valor de Desconto
	COALESCE(VR_PAGO, 0) + COALESCE(VR_JUROS, 0) + COALESCE(VR_MULTA, 0) - COALESCE(VR_DESCONTO, 0),			
	'| ',		-- Valor Baixado
	COALESCE(CAST(CLIDOC.NOTA_FISCAL AS VARCHAR(100)), 'NULL'),		
	'| ',		-- Numero da Nota Fiscal ou Cupom Fiscal
	COALESCE(CAST(CLIDOC.FATURA AS VARCHAR(20)), ''),			
	'| ',		-- Numero da Fatura
	CAST(COALESCE(CLIDOC.DUPLICATA, '') AS VARCHAR(20)),			
	'| ',		-- Numero da Duplicat
	REPLACE(REPLACE(
		COALESCE(CLIDOC.TEXTO, '') +
		CASE
			when
				CLIDOC.DT_VENCIMENTO < CLIDOC.DT_EMISSAO
				then  ' [VENCIMENTO AJUSTADO DE ' + CONVERT(VARCHAR(10), CLIDOC.DT_VENCIMENTO, 103) + ' PARA ' + CONVERT(VARCHAR(10), CLIDOC.DT_EMISSAO, 103) + ']'
				else ''
		end,
		CHAR(13), ''), CHAR(10), ''),			
	'| ',		-- Observacoes
	CLIDOC.ATIVO -- Ativo
FROM CLIDOC 
LEFT JOIN CADPOR ON CADPOR.PORTADOR = CLIDOC.PORTADOR
WHERE CLIDOC.VR_PARCELA > COALESCE(CLIDOC.VR_PAGO, 0) AND CLIDOC.ATIVO = 1 and clidoc.filial = @filial
";

                    SqlCommand command = new SqlCommand(query, cn);
                    command.Parameters.Add(new SqlParameter("@filial", SqlDbType.NVarChar) { Value = filial });
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        public void Fordoc(string LocalDiretorio, string filial)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT
	1,
	'| ',		-- Codigo da Filial
	CASE WHEN CADPOR.DEBITO = 'N' AND CADPOR.CREDITO = 'S' THEN '1' ELSE '2' END,			
	'| ',		-- Tipo do Documento 1 - Receber e 2 - Pagar
	FORDOC.FORNECEDOR ,			
	'| ',		-- Codigo do Cliente
	COALESCE(FORDOC.DOCUMENTO, 0),		
	'| ',		-- Numero do Documento
	COALESCE(FORDOC.PARCELA, ''),		
	'| ',		-- Numero da Parcela
	CONVERT(VARCHAR(100), FORDOC.DT_EMISSAO, 103),			
	'| ',		-- Data de Emissao
	CONVERT(VARCHAR(100), FORDOC.DT_VENCIMENTO, 103),			
	'| ',		-- Data de Vencimento
	CONVERT(VARCHAR(100), FORDOC.DT_PAGAMENTO, 103),		
	'| ',		-- Data de Baixa
	CONVERT(VARCHAR(100), FORDOC.DT_PRORROGACAO, 103),			
	'| ',		-- Data de Prorrogacao
	CAST(COALESCE(FORDOC.VR_PARCELA, 0) AS VARCHAR(100)),			
	'| ',		-- Valor da Parcela
	COALESCE(FORDOC.VR_JUROS, 0),			
	'| ',		-- Valor de Juros
	COALESCE(FORDOC.VR_MULTA, 0),			
	'| ',		-- Valor de Multa
	COALESCE(FORDOC.VR_DESCONTO, 0),			
	'| ',		-- Valor de Desconto
	COALESCE(VR_PAGO, 0) + COALESCE(VR_JUROS, 0) + COALESCE(VR_MULTA, 0) - COALESCE(VR_DESCONTO, 0),			
	'| ',		-- Valor Baixado
	COALESCE(CAST(FORDOC.NOTA_FISCAL AS VARCHAR(100)), 'NULL'),		
	'| ',		-- Numero da Nota Fiscal ou Cupom Fiscal
	'',			
	'| ',		-- Numero da Fatura
	CAST(COALESCE(FORDOC.DUPLICATA, '') AS VARCHAR(20)),			
	'| ',		-- Numero da Duplicat
	REPLACE(REPLACE(COALESCE(FORDOC.TEXTO, ''), CHAR(13), ''), CHAR(10), ''),			
	'| ',		-- Observacoes
	1
				-- Ativo

FROM FORDOC 
LEFT JOIN CADPOR ON CADPOR.PORTADOR = FORDOC.PORTADOR
WHERE FORDOC.VR_PARCELA > COALESCE(FORDOC.VR_PAGO, 0) and fordoc.filial = @filial
";

                    SqlCommand command = new SqlCommand(query, cn);
                    command.Parameters.Add(new SqlParameter("@filial", SqlDbType.NVarChar) { Value = filial });
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        //metodo replicar saldo filial 2

        public void GradeProdutoReplicarFilial1(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT 
	CADPRO.ID AS PRODUTOID, 
	'|',
	1,
	'|',
	CASE
		WHEN LEN(LTRIM(RTRIM(GRADE))) > 0
			THEN LTRIM(RTRIM(GRADE))
		ELSE 'U'
	END AS GRADE,
	'|',
	ESTGRA.ESTOQUEFISICO AS QUANTIDADE,
	'|',
	case ESTGRA.ATIVO when 'true' then 1 else 0 end
FROM CADPRO
JOIN ESTSAL ON ESTSAL.MATRICULA = CADPRO.ID
JOIN ESTGRA ON ESTGRA.IDESTSAL = ESTSAL.ID
WHERE CADPRO.ID > 1 and estsal.filial = 1

union all

SELECT 
	CADPRO.ID AS PRODUTOID, 
	'|',
	2,
	'|',
	CASE
		WHEN LEN(LTRIM(RTRIM(GRADE))) > 0
			THEN LTRIM(RTRIM(GRADE))
		ELSE 'U'
	END AS GRADE,
	'|',
	ESTGRA.ESTOQUEFISICO AS QUANTIDADE,
	'|',
	case ESTGRA.ATIVO when 'true' then 1 else 0 end
FROM CADPRO
JOIN ESTSAL ON ESTSAL.MATRICULA = CADPRO.ID
JOIN ESTGRA ON ESTGRA.IDESTSAL = ESTSAL.ID
WHERE CADPRO.ID > 1 and estsal.filial = 1

";

                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        public void SaldoReplicarFilial1(string LocalDiretorio)
        {
            try
            {
                // Verificar se o diretório existe e garantir permissões
                string directory = Path.GetDirectoryName(LocalDiretorio);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (SqlConnection cn = new SqlConnection(Conn2.StrCon))
                {
                    cn.Open();
                    string query = @"SELECT 
	CADPRO.ID,
	'|',
	1,
	'|',
	1 AS TABELAPRECO,
	'|',
	NULL, --TABELAMARKUP.CODIGOMARKUP AS MARKUP,
	'|',
	COALESCE(ESTSAL.VRVENDAV, 0),
	'|',
	COALESCE(ESTSAL.VRCOMPRAV, 0),
	'|',
	COALESCE(ESTSAL.VRCUSTOV, 0),
	'|',
	COALESCE(ESTSAL.VRZEROV, 0),
	'|',
	COALESCE(ESTSAL.VRMEDIOV, 0),
	'|',
	COALESCE(ESTSAL.LUCROV, 0),
	'|',
	case ESTSAL.ATIVO when 'true' then 1 else 0 end
 FROM CADPRO
 JOIN ESTSAL ON ESTSAL.MATRICULA = CADPRO.ID
 LEFT JOIN (
	SELECT
	ROW_NUMBER() OVER(ORDER BY DATA) + 1 AS CODIGOMARKUP /* Markup 1 ja existe no banco da web */, CADMKP.ID, CADMKP.NOME, CADMKP.PERCENTUAL, CADMKP.ATIVO FROM CADMKP
 ) AS TABELAMARKUP ON TABELAMARKUP.ID = ESTSAL.IDCADMKP
 WHERE CADPRO.ID > 1 and estsal.filial = 1
 
union all 

SELECT 
	CADPRO.ID,
	'|',
	2,
	'|',
	1 AS TABELAPRECO,
	'|',
	NULL, --TABELAMARKUP.CODIGOMARKUP AS MARKUP,
	'|',
	COALESCE(ESTSAL.VRVENDAV, 0),
	'|',
	COALESCE(ESTSAL.VRCOMPRAV, 0),
	'|',
	COALESCE(ESTSAL.VRCUSTOV, 0),
	'|',
	COALESCE(ESTSAL.VRZEROV, 0),
	'|',
	COALESCE(ESTSAL.VRMEDIOV, 0),
	'|',
	COALESCE(ESTSAL.LUCROV, 0),
	'|',
	case ESTSAL.ATIVO when 'true' then 1 else 0 end
 FROM CADPRO
 JOIN ESTSAL ON ESTSAL.MATRICULA = CADPRO.ID
 LEFT JOIN (
	SELECT
	ROW_NUMBER() OVER(ORDER BY DATA) + 1 AS CODIGOMARKUP /* Markup 1 ja existe no banco da web */, CADMKP.ID, CADMKP.NOME, CADMKP.PERCENTUAL, CADMKP.ATIVO FROM CADMKP
 ) AS TABELAMARKUP ON TABELAMARKUP.ID = ESTSAL.IDCADMKP
 WHERE CADPRO.ID > 1 and estsal.filial = 1
";

                    SqlCommand command = new SqlCommand(query, cn)
                    {
                        CommandTimeout = 600
                    };
                    SqlDataReader reader = command.ExecuteReader();

                    StringBuilder resultStringBuilder = new StringBuilder();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultStringBuilder.Append(reader[i].ToString());
                        }
                        resultStringBuilder.AppendLine(); // Nova linha para cada registro
                    }

                    File.WriteAllText(LocalDiretorio, resultStringBuilder.ToString());
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
    }
}

