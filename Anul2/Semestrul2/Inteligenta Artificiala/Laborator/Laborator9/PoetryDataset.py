import torch
from torch.utils.data import Dataset

class PoetryDataset(Dataset):
    def __init__(self, samples, tokenizer, max_length=600):
        self.input_ids = []

        for sample in samples:
            # tokenizare
            output = tokenizer.apply_chat_template(
                sample,
                tokenize=True,
                add_generation_prompt=False,  # pentru antrenare putem sa-l lasam ca fals
                truncation=True,
                max_length=max_length,
                padding="max_length"
            )
            if isinstance(output, dict) or hasattr(output, 'input_ids'):
                ids = output['input_ids']
            else:
                ids = output

            self.input_ids.append(list(ids))

    def __len__(self):
        return len(self.input_ids)

    def __getitem__(self, idx):
        # se returneaza un dicționar pe care modelele Hugging Face il inteleg nativ
        ids = torch.tensor(self.input_ids[idx], dtype=torch.long)
        return {"input_ids": ids, "labels": ids}